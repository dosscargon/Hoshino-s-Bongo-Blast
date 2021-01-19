using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common;
using System.IO;
using System.Reflection;
using UnityEngine.SceneManagement;

public class SlectSongTweets : MenuButton {
    private Animator animator;
    private AudioSource audioSource;

    /// <summary>
    /// 選択可能な楽曲データ
    /// </summary>
    [SerializeField]
    private List<MusicData> musicDatas;
    static public List<MusicData> staticMusicDatas;
    /// <summary>
    /// 曲の情報を表示するオブジェクト
    /// </summary>
    [SerializeField]
    private List<Tweet> tweets;

    [SerializeField]
    private Text versionText;

    /// <summary>
    /// 曲IDに紐づけられたクリア情報
    /// </summary>
    private Dictionary<string, ClearData> ClearDatas = new Dictionary<string, ClearData>();
    /// <summary>
    /// 曲IDに紐づけられたハイスコア情報 
    /// </summary>
    private Dictionary<string, int> HiScores=new Dictionary<string, int>();

    public static int _selectedNumber = 0;
    /// <summary>
    /// 何番目の曲を選択中か
    /// </summary>
    public int SelectedNumber {
        get => _selectedNumber;
        set {
            _selectedNumber = (value + staticMusicDatas.Count) % staticMusicDatas.Count;
            for (int i = 0; i < tweets.Count; i++) {
                tweets[i].MusicData = staticMusicDatas[((i + _selectedNumber - tweets.Count / 2) + staticMusicDatas.Count) % staticMusicDatas.Count];
                tweets[i].SetInfo(ClearDatas[tweets[i].MusicData.MusicID], HiScores[tweets[i].MusicData.MusicID]);
            }

            audioSource.clip = tweets[2].MusicData.Clip;
            audioSource.time = tweets[2].MusicData.DemoStart;
            audioSource.volume = GameManager.Instance.MasterVolume / 100f * tweets[2].MusicData.Volume;
            audioSource.Play();
        }
    }

    private void Awake() {
        //versionText.text = Application.dataPath;
    }

    // Use this for initialization
    void Start () {
        FirstFocus();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        //GameManager.Instance.init();

        //セーブデータ読み込み
        foreach (var musicData in staticMusicDatas) {
            ClearDatas[musicData.MusicID] = (ClearData)PlayerPrefs.GetInt(musicData.MusicID + ".ClearData", 0);
            HiScores[musicData.MusicID] = PlayerPrefs.GetInt(musicData.MusicID + ".HiScore", 0);
        }

        SelectedNumber = _selectedNumber;

        //versionText.text = GameManager.Version;
#if DEBUG_ON
        versionText.text += "(DEBUG MODE)";
#endif
    }
	
	// Update is called once per frame
	void Update () {
        if (!audioSource.isPlaying) {
            audioSource.time = tweets[2].MusicData.DemoStart;
            audioSource.Play();
        }
        if (Focus) {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle")) {
                if (Inputmanager.GetKeyUp()) {
                    animator.SetTrigger("Down");
                    SelectedNumber--;
                } else if (Inputmanager.GetKeyDown()) {
                    animator.SetTrigger("Up");
                    SelectedNumber++;
                }

                Inputkey();
#if UNITY_ANDROID
                if (Input.touchCount>0) {
                    int touchNumber = (int)Input.touches[0].position.y / (Screen.height / 3);
                    if (touchNumber == 0) {
                        animator.SetTrigger("Up");
                        SelectedNumber++;
                    } else if (touchNumber == 2) { 
                        animator.SetTrigger("Down");
                        SelectedNumber--;
                    }else if (touchNumber == 1) {
                        StartGame();
                    }
                }
#endif
            }
        }
	}

    /// <summary>
    /// ゲーム開始
    /// </summary>
    public void StartGame() {
        GameManager.Instance.PlayingMusicHiScore = HiScores[tweets[2].MusicData.MusicID];
        GameManager.Instance.StartGame(tweets[2].MusicData);
    }
}
