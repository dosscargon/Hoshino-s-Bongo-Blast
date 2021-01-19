using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KoukaiButtonYes : MonoBehaviour {
    public Text label;

    /// <summary>
    /// 念押しメッセージ
    /// </summary>
    [SerializeField]
    [Multiline]
    public List<string> messages=new List<string>() { };

    private int counter = 0;

	// Use this for initialization
	void Start () {
        if(messages.Count==0) {
            messages.Add("メッセージが入ってないやん！");
        }
        label.text = messages[0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void YesButton_Click() {
        counter++;
        if(counter<messages.Count) {
            label.text = messages[counter];
        }else {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Title");
        }
    }
}
