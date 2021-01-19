using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour {
    [SerializeField]
    private Text musicVolumeText;
    [SerializeField]
    private Text seVolumeText;

    private void Start() {
        musicVolumeText.text = "音量:" + GameManager.Instance.MasterVolume.ToString();
        seVolumeText.text = "効果音:" + GameManager.Instance.SEVolume.ToString();

    }

    public void MoveMusicVolume() {
        if (GameManager.Instance.MasterVolume != 0) {
            GameManager.Instance.MasterVolume -= 10;
        } else {
            GameManager.Instance.MasterVolume = 100;
        }
        musicVolumeText.text = "音量:" + GameManager.Instance.MasterVolume.ToString();
    }
    public void MoveSEVolume() {
        if (GameManager.Instance.SEVolume != 0) {
            GameManager.Instance.SEVolume -= 10;
        } else {
            GameManager.Instance.SEVolume = 100;
        }
        seVolumeText.text = "効果音:" + GameManager.Instance.SEVolume.ToString();
    }

    public void SaveOption() {
        PlayerPrefs.SetInt("MusicVolume", GameManager.Instance.MasterVolume);
        PlayerPrefs.SetInt("SEVolume", GameManager.Instance.SEVolume);
        PlayerPrefs.SetInt("KeyConfig", GameManager.Instance.KeyConfig);
    }
}
