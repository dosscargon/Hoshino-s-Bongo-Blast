using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.Common;

public class LaneController : MonoBehaviour {
    /// <summary>
    /// プレイ中の曲のBPM
    /// </summary>
    public float Bpm { get; set; }
    /// <summary>
    /// プレイ中の曲は何拍子か
    /// </summary>
    public float Measure { get; set; } = 4;
    /// <summary>
    /// 曲と譜面の開始時間の差
    /// </summary>
    public float Offset { get; set; }
    private float speed = 1;
    /// <summary>
    /// 譜面スピード
    /// </summary>
    public float Speed {
        get => speed;
        set {
            if (value != 0) {
                speed = value;
            }
        }
    }

    //判定の長さ
    [SerializeField]
    private float judgeRangeGreat;
    public float JudgeRangeGreat {
        get {
            return judgeRangeGreat;
        }
    }
    [SerializeField]
    private float judgeRangeGood;
    public float JudgeRangeGood {
        get {
            return judgeRangeGood;
        }
    }
    [SerializeField]
    private float judgeRangeBad;
    public float JudgeRangeBad {
        get {
            return judgeRangeBad;
        }
    }

    //判定ごとのスコア増加量
    [SerializeField]
    private int greatScore;
    [SerializeField]
    private int goodScore;

    //判定ごとの通知増加倍率．1だとフルコンしたときに10000になる
    [SerializeField]
    private float greatBuzzMagnification;
    [SerializeField]
    private float goodBuzzMagnification;

    [SerializeField]
    private float waitTime;


    //有効なノーツ
    private List<GameObject> activeNotes = new List<GameObject>();
    public float EndMarker { get; set; }

    #region UI
    [SerializeField]
    private Image ClapperBody;
    [SerializeField]
    private Animator[] leftArmAnimator;
    [SerializeField]
    private Animator[] rightArmAnimator;
    [SerializeField]
    private Text judgeText;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private FlexibleText buzzText;
    [SerializeField]
    private Text comboText;
    //未使用．歌詞を表示予定
    [SerializeField]
    private Text lyricText;
    #endregion

    private AudioSource audioSource;

    /// <summary>
    /// シーン開始時刻
    /// </summary>
    private float startTime;
    /// <summary>
    /// 曲の再生を始めた？
    /// </summary>
    private bool isPlaying = false;

    /// <summary>
    /// ノーツの量．通知増加量の計算に用いる
    /// </summary>
    private int amountNotes;

    /// <summary>
    /// 四分音符の長さを計算する
    /// </summary>
    public float BeatLength {
        get {
            return 60 / Bpm;
        }
    }

    /// <summary>
    /// 再生時間
    /// </summary>
    public float AudioTime {
        get {
            if (audioSource.isPlaying) {
                return audioSource.time;
            } else {
                return -(waitTime - (Time.time - startTime));
            }
        }
    }

    private void ChangeCharacter() {
        //ClapperBody.sprite = (Sprite)Resources.Load($"Clappers\\{characterID}\\body",typeof(Sprite));
        CharacterData clapper = new CharacterData(GameManager.Instance.ClapperID);

        ClapperBody.sprite = clapper.Body;
        leftArmAnimator[1].GetComponent<ImageChanger>().sprites = clapper.LeftArm;
        leftArmAnimator[1].GetComponent<Image>().sprite = clapper.LeftArm[0];
        rightArmAnimator[1].GetComponent<ImageChanger>().sprites = clapper.RightArm;
        rightArmAnimator[1].GetComponent<Image>().sprite = clapper.RightArm[0];

        if (clapper.CharacterID == "KamuNi_Ai" || clapper.CharacterID == "Tohu") {
            GetComponents<AudioSource>()[1].clip = (AudioClip)Resources.Load("Sounds\\KamuNiDrum", typeof(AudioClip));
        }
    }

    // Use this for initialization
    void Start() {

        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        //ClapperBody.sprite = GameManager.Instance.Clapper.Body;
        //leftArmAnimator[1].GetComponent<ImageChanger>().sprites = GameManager.Instance.Clapper.LeftArm;
        //rightArmAnimator[1].GetComponent<ImageChanger>().sprites = GameManager.Instance.Clapper.RightArm;

        ChangeCharacter();

        audioSource = GetComponents<AudioSource>()[0];
        audioSource.clip = GameManager.Instance.PlayingMusicData.Clip;

        audioSource.volume = GameManager.Instance.MasterVolume / 100f;
        GetComponents<AudioSource>()[1].volume = GameManager.Instance.SEVolume / 100f * GameManager.MaxSEVolume;

        //譜面情報を取得
        NotesLoader loader = new NotesLoader(GameManager.Instance.PlayingMusicData.Txt, this);
        loader.Load();

        foreach (var note in activeNotes) {
            //ノーツの数を数える
            if (note.GetComponent<Note>().noteType != NoteType.Bar) {
                amountNotes++;
            }
        }

        //時間計測用
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update() {
#if UNITY_ANDROID
        if (!GameManager.Instance.IsAuto) {
            foreach (var touch in Input.touches) {
                if (touch.phase == TouchPhase.Began) {
                    int touchNumber = (int)touch.position.x / (Screen.width / 4);
                    lyricText.text = $"Android Mode\nWidth:{Screen.width}\ntouchPos_x:{touch.position.x}\ntouchNumber:{touchNumber}";

                    if (GameManager.Instance.KeyConfig == 0) {
                        if (touchNumber == 0 || touchNumber == 2) {
                            ClapLeft();
                        }
                        if (touchNumber == 1 || touchNumber == 3) {
                            ClapRight();
                        }
                    } else if (GameManager.Instance.KeyConfig == 1) {
                        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F)) {
                            ClapLeft();
                        }
                        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K)) {
                            ClapRight();
                        }
                    }
                }
            }
        }

            //戻る
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MusicSelect");
        }
#endif
        if (!GameManager.Instance.IsAuto) {
            if (GameManager.Instance.KeyConfig == 0) {
                if (Inputmanager.GetKeyDownD() || Inputmanager.GetKeyDownJ()) {
                    ClapLeft();
                }
                if (Inputmanager.GetKeyDownF() || Inputmanager.GetKeyDownK()) {
                    ClapRight();
                }
            } else if (GameManager.Instance.KeyConfig == 1) {
                if (Inputmanager.GetKeyDownD() || Inputmanager.GetKeyDownF()) {
                    ClapLeft();
                }
                if (Inputmanager.GetKeyDownJ() || Inputmanager.GetKeyDownK()) {
                    ClapRight();
                }
            }
        }

        //戻る
        if (Inputmanager.GetKeyDownQuit()) {
            SceneManager.LoadScene("MusicSelect");
        }

        ////（デバッグ用）
        if (Input.GetKey(KeyCode.Semicolon)) {
            //audioSource.time += BeatLength;
            audioSource.pitch = 3;
        } else if (Input.GetKey(KeyCode.Slash)) {
            audioSource.pitch = 0.25f;
        } else {
            audioSource.pitch = 1;
        }

        //時間になったら曲を始める
        if (!isPlaying && AudioTime >= 0) {
            audioSource.volume = (GameManager.Instance.MasterVolume / 100f) * GameManager.Instance.PlayingMusicData.Volume;
            audioSource.Play();
            isPlaying = true;
        }

        //曲が終わったらリザルトへ
        if (GameManager.Instance.PlayingMusicData.Clip != null) {
            if (AudioTime >= EndMarker || (!audioSource.isPlaying && isPlaying)) {
                SceneManager.LoadScene("Result");
            }
        }

#if DEBUG_ON
        lyricText.text = $"DEBUG MODE\nFPS:{1f / Time.deltaTime:0000.00}\ntime:{AudioTime}";
#endif
    }

    /// <summary>
    /// ノーツを登録する
    /// </summary>
    /// <param name="noteType">登録するノーツの種類</param>
    /// <param name="beats">何小節目?</param>
    public void RegistNotes(NoteType noteType, float beats) {
        GameObject noteGameObject;

        switch (noteType) {
            case NoteType.Like:
                noteGameObject = Instantiate((GameObject)Resources.Load("Prefabs/LikesNote"), transform);
                break;
            case NoteType.RT:
                noteGameObject = Instantiate((GameObject)Resources.Load("Prefabs/RTNote"), transform);
                break;
            default:
                noteGameObject = Instantiate((GameObject)Resources.Load("Prefabs/MeasureBar"), transform);
                break;
        }

        Note note = noteGameObject.GetComponent<Note>();

        note.LaneManagerGameobject = gameObject;
        note.appearanceTime = beats - BeatLength * Measure + Offset;
        note.targetTime = beats + Offset;
        note.Speed = Speed;

        activeNotes.Add(noteGameObject);

    }

    /// <summary>
    /// ノーツを削除する
    /// </summary>
    public void DestroyNotes(Note note) {
        activeNotes.Remove(note.gameObject);
        note.gameObject.SetActive(false);
        //Destroy(note.gameObject);
    }

    /// <summary>
    /// 右がたたかれた
    /// </summary>
    public void ClapRight() {
        HitNote(NoteType.Like);

        foreach (var animator in rightArmAnimator) {
            animator.SetTrigger("Clap");
        }
        GetComponents<AudioSource>()[1].Play();
        //GetComponents<AudioSource>()[1].time = 0;

    }

    /// <summary>
    /// 左がたたかれた
    /// </summary>
    public void ClapLeft() {
        HitNote(NoteType.RT);

        foreach (var animator in leftArmAnimator) {
            animator.SetTrigger("Clap");
        }
        GetComponents<AudioSource>()[1].Play();
        //GetComponents<AudioSource>()[1].time = 0;

    }

    /// <summary>
    /// ノーツをたたいたときの処理
    /// </summary>
    /// <param name="noteType"></param>
    void HitNote(NoteType noteType) {
        //noteTypeと一致するノーツを抽出
        //List<GameObject> matchingNotes = new List<GameObject>(activeNotes.FindAll(n => n.GetComponent<Note>().noteType == noteType));
        //判定に最も近い順にソート
        //matchingNotes.Sort((a, b) => (Mathf.Abs(a.GetComponent<Note>().targetTime - AudioTime).CompareTo(Mathf.Abs(b.GetComponent<Note>().targetTime - AudioTime))));

        //foreach(var noteGameObject in matchingNotes) {
        for(int i = 0; i < activeNotes.Count; i++) { 
        //if (matchingNotes.Count != 0) {
            Note note = activeNotes[i].GetComponent<Note>();
            if (note.noteType == noteType) {
                float dif = Mathf.Abs(note.targetTime - AudioTime);
                if (dif <= judgeRangeBad && note.targetTime - AudioTime >= -JudgeRangeGood) {
                    if (dif <= judgeRangeGreat) {
                        AddScore(Judge.Great);
                    } else if (dif <= judgeRangeGood) {
                        AddScore(Judge.Good);
                    } else {
                        AddScore(Judge.Bad);
                    }
                    DestroyNotes(note);
                    break;
                }
            }
        }

    }

    /// <summary>
    /// 判定に基づいたスコア処理
    /// </summary>
    /// <param name="judge">判定</param>
    public void AddScore(Judge judge) {
        if (!GameManager.Instance.IsAuto) {
            if (judge == Judge.Great) {
                judgeText.text = "Great!";
                GameManager.Instance.Score += greatScore;
                GameManager.Instance.Buzz += (GameManager.buzzGoal / (float)amountNotes) * greatBuzzMagnification;
                GameManager.Instance.Combo++;
            } else if (judge == Judge.Good) {
                judgeText.text = "Good";
                GameManager.Instance.Score += goodScore;
                GameManager.Instance.Buzz += GameManager.buzzGoal / (float)amountNotes * goodBuzzMagnification;
                GameManager.Instance.Combo++;
            } else {
                judgeText.text = "Bad...";
                GameManager.Instance.Combo = 0;
            }
        } else {
            judgeText.text = "Auto";
            GameManager.Instance.Score += greatScore;
            GameManager.Instance.Buzz += (GameManager.buzzGoal / (float)amountNotes) * greatBuzzMagnification;
            GameManager.Instance.Combo++;
        }
        if (!GameManager.Instance.IsAuto) {
            GameManager.Instance.JudgeCountPlus(judge);
        } else {
            GameManager.Instance.JudgeCountPlus(Judge.Great);
        }
        buzzText.Text = ((int)(GameManager.Instance.Buzz)).ToString();
        scoreText.text = "Score :\n" + GameManager.Instance.Score.ToString();
        judgeText.GetComponent<Animator>().SetTrigger("Judge");
        comboText.text = GameManager.Instance.Combo.ToString() + "\nCombo!";
    }
}