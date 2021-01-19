using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleText : MonoBehaviour {
    #region UI
    private Text textComponent;
    private RectTransform rTransformComponent;
    #endregion

    /// <summary>
    /// 何文字目からデカくし始める？
    /// </summary>
    [SerializeField]
    private int ignoireLength;
    /// <summary>
    /// 最初のサイズは？
    /// </summary>
    [SerializeField]
    private float firstScale;
    /// <summary>
    /// 1文字ごとにどれくらいデカくなる？
    /// </summary>
    [SerializeField]
    private float scalePerCharacter;
    
    private string text;
    /// <summary>
    /// 表示するテキスト．文字列の長さでサイズが変わる
    /// </summary>
    public string Text {
        get => text;
        set {
            text = value;
            textComponent.text = text;
            rTransformComponent.sizeDelta = new Vector3(
                DecideScale(text),
                rTransformComponent.sizeDelta.y
            );
        }
    }

    private float DecideScale(string text) {
        if (text.Length <= ignoireLength) {
            return firstScale;
        } else {
            return firstScale + (text.Length - 2) * scalePerCharacter;
        }
    }

	// Use this for initialization
	void Start () {
        textComponent = GetComponent<Text>();
        rTransformComponent = GetComponent<RectTransform>();
	}
}
