using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        RefrectText();
    }

    private void RefrectText() {
        if (GameManager.Instance.IsAuto) {
            gameObject.GetComponentInChildren<Text>().GetComponent<Text>().text = "オート\nON";
        } else {
            gameObject.GetComponentInChildren<Text>().GetComponent<Text>().text = "オート\nOFF";
        }
    }

    public void SwitchAuto() {
        GameManager.Instance.IsAuto = !GameManager.Instance.IsAuto;
        RefrectText();
    }
}
