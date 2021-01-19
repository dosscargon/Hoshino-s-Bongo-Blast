using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common;

public class KeyConfig : MonoBehaviour {
    [SerializeField]
    private Text keyTypeText;

    [SerializeField]
    private Image dKeyImage;
    [SerializeField]
    private Image fKeyImage;
    [SerializeField]
    private Image jKeyImage;
    [SerializeField]
    private Image kKeyImage;
    [SerializeField]
    private Image joystickImage;

    [SerializeField]
    private List<Sprite> keyTypeSplites;

    Color likeColor = new Color(0.8784314f, 0.1411765f, 0.3686275f);
    Color rtColor = new Color(0.09019608f, 0.7490196f, 0.3882353f);

    private void Start() {
        DisplayKeyConfig();
    }

    public void ChangeKeyConfig() {
        GameManager.Instance.KeyConfig = (GameManager.Instance.KeyConfig + 1) % 2;
        DisplayKeyConfig();
    }

    private void DisplayKeyConfig() {
        keyTypeText.text = "操作タイプ:";
        if (GameManager.Instance.KeyConfig == 0) {
            keyTypeText.text += "A";
            dKeyImage.color = rtColor;
            fKeyImage.color = likeColor;
            jKeyImage.color = rtColor;
            kKeyImage.color = likeColor;
        } else if (GameManager.Instance.KeyConfig == 1) {
            keyTypeText.text += "B";
            dKeyImage.color = rtColor;
            fKeyImage.color = rtColor;
            kKeyImage.color = likeColor;
            jKeyImage.color = likeColor;
        }

        joystickImage.sprite = keyTypeSplites[GameManager.Instance.KeyConfig];
    }


    public void SaveOption() {
        PlayerPrefs.SetInt("KeyConfig", GameManager.Instance.KeyConfig);
    }
}
