using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Common;
using UnityEngine.SceneManagement;

public class DifficultySelect : MonoBehaviour {
    /// <summary>
    /// 選択可能な楽曲データ
    /// </summary>
    [SerializeField]
    private List<MusicData> musicDatas;
    [SerializeField]
    private int difficulty;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void LoadDifficulty() {
        SlectSongTweets.staticMusicDatas = musicDatas;
        SlectSongTweets._selectedNumber = 0;
        GameManager.Instance.Difficulty = difficulty;
        //SceneManager.LoadScene("MusicSelect");
    }
}
