using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tweet : MonoBehaviour {
    #region UI
    private Image iconImage;
    private Text titleText;
    private Text memoText;
    private Text infoText;
    #endregion

    private MusicData data;
    /// <summary>
    /// 音楽データを表示に反映させる
    /// </summary>
    public MusicData MusicData {
        get => data;
        set {
            data = value;
            iconImage.sprite = MusicData.Icon;
            titleText.text = data.Title;
            memoText.text = data.Memo;
        }
    }

    /// <summary>
    /// クリア情報とかを表示に反映させる
    /// </summary>
    public void SetInfo(ClearData clearData,int hiScore) {
        switch (clearData) {
            case ClearData.Cleared:
                infoText.text = "Cleared!\n";
                break;
            case ClearData.FullCombo:
                infoText.text = "Perfect!!!\n";
                break;
            default:
                infoText.text = "\n";
                break;
        }

        infoText.text += "High Score : " + hiScore.ToString();
    }

    void Start() {
        iconImage = transform.GetChild(1).GetComponent<Image>();
        titleText = transform.GetChild(2).GetComponent<Text>();
        memoText = transform.GetChild(3).GetComponent<Text>();
        infoText = transform.GetChild(4).GetComponent<Text>();
    }
}
