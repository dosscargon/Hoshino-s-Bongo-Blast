using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Assets.Scripts.Common;

public class ResultController : MonoBehaviour {
    ClearData clearData;

    private string[] difficultyText ={
        "やさしい",
        "ふつう",
        "たいへん",
        "すごくたいへん"
    };

    [SerializeField]
    private float scoreWaitTime;

    #region UI
    [SerializeField]
    private FlexibleText scoreText;
    [SerializeField]
    private GameObject clearText;
    [SerializeField]
    private Animator tweetAnim;
    [SerializeField]
    private Animator naviAnim;
    
    [SerializeField]
    private Tweet tweet;
    [SerializeField]
    private Text resultText;
    [SerializeField]
    private Text infoText;
    [SerializeField]
    private Text versionText;
    #endregion

    private bool isRecieving = false;

	// Use this for initialization
	void Start () {
        DisplayData();
        if (!GameManager.Instance.IsAuto) {
            SaveData();
        }

        versionText.text = GameManager.Version;
        StartCoroutine("ControlResult");
#if DEBUG_ON
        versionText.text += "(DEBUG MODE)";
#endif
    }

    /// <summary>
    /// リザルト情報を表示
    /// </summary>
    private void DisplayData() {
        tweet.MusicData = GameManager.Instance.PlayingMusicData;
        resultText.text =
            "Score : \t" + GameManager.Instance.Score + "\n\n" +
            "Great :\t" + GameManager.Instance.getJudgeCount(Judge.Great).ToString() + "\n" +
            "Good :\t" + GameManager.Instance.getJudgeCount(Judge.Good).ToString() + "\n" +
            "Bad :\t" + GameManager.Instance.getJudgeCount(Judge.Bad).ToString() + "\n\n" +
            "Max Combo : " + GameManager.Instance.MaxCombo.ToString();

        if (!GameManager.Instance.IsAuto) {
            if (GameManager.Instance.getJudgeCount(Judge.Bad) == 0) {
                clearText.GetComponent<Text>().text = "Perfect!!!";
                clearData = ClearData.FullCombo;
            } else if (GameManager.Instance.Buzz >= GameManager.buzzGoal) {
                clearText.GetComponent<Text>().text = "Clear!";
                clearData = ClearData.Cleared;
            } else {
                clearText.GetComponent<Text>().text = "Failure...";
                clearData = ClearData.NotCleared;
            }
        } else {
            clearText.GetComponent<Text>().text = "Auto";
            clearData = ClearData.NotCleared;
        }

        infoText.text = "\nHigh Score : " + GameManager.Instance.PlayingMusicHiScore;
    }

    /// <summary>
    /// データを保存する
    /// </summary>
    private void SaveData() {
        if (GameManager.Instance.PlayingMusicClearData < clearData) {
            PlayerPrefs.SetInt(GameManager.Instance.PlayingMusicData.MusicID + ".ClearData", (int)clearData);
        }
        if (GameManager.Instance.PlayingMusicHiScore < GameManager.Instance.Score) {
            PlayerPrefs.SetInt(GameManager.Instance.PlayingMusicData.MusicID + ".HiScore", GameManager.Instance.Score);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isRecieving) {
#if UNITY_ANDROID
            if (Input.touchCount > 0) {
                SceneManager.LoadScene("MusicSelect");
            }
#endif
            if (Inputmanager.GetKeyDownSubmit()) {
                SceneManager.LoadScene("MusicSelect");
            } else if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(Inputmanager.ButtonX)) {
                GoTwitter();
            }
        }
	}

    private IEnumerator ControlResult() {
        //通知の数を増やしながら表示
        while (Time.timeSinceLevelLoad < scoreWaitTime) {
            scoreText.Text = ((int)(GameManager.Instance.Buzz * (Time.timeSinceLevelLoad / scoreWaitTime))).ToString();
            yield return null;
        }
        scoreText.Text = ((int)GameManager.Instance.Buzz).ToString();

        //出し終わったら各種表示を登場させる
        clearText.GetComponent<Animator>().SetTrigger("ScoreEnd");
        tweetAnim.SetTrigger("ScoreEnd");
        naviAnim.SetTrigger("ScoreEnd");
        yield return new WaitForSeconds(1);

        //入力受付開始
        isRecieving = true;
    }

    /// <summary>
    /// 結果のツイート画面を出す
    /// </summary>
    private void GoTwitter() {
        Cursor.visible = true;
        string text = "";
        if (!GameManager.Instance.IsAuto) {
            if (clearData == ClearData.FullCombo) {
                text = "パーフェクト！ ";
            } else if (clearData == ClearData.Cleared) {
                text = "ノルマクリア！ ";
            }
        } else {
            text = "オートモード！";
        }
        text += $"「{GameManager.Instance.PlayingMusicData.Title}」（{difficultyText[GameManager.Instance.Difficulty]}）で{GameManager.Instance.Score}点獲得した！";

        string url =  $"https://twitter.com/intent/tweet?text={text}&hashtags=星野さんのらぶりつ大作戦&related=dosscargon&via=dosscargon";

        //Process.Start(url);
        Application.OpenURL(url);
    }

    private void OnApplicationFocus(bool focus) {
        if (!focus) {
            Cursor.visible = false;
        }
    }


}
