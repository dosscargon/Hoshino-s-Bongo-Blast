using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour {
    [SerializeField]
    private int characterNumber;

    [SerializeField]
    private CharacterData characterData;

    [SerializeField]
    private string characterName;

    [SerializeField]
    private string characterRight;

    [SerializeField]
    [Multiline]
    private string characterMemo;

    public void ShowCharaInfo() {
        GameObject characterTexts = GameObject.Find("CharacterTexts");
        characterTexts.transform.GetChild(1).GetComponent<Text>().text = characterName;
        characterTexts.transform.GetChild(2).GetComponent<Text>().text = characterRight;
        characterTexts.transform.GetChild(3).GetComponent<Text>().text = characterMemo;
    }

    public void SetCharacter() {
        //GameManager.Instance.Clapper = characterData;
        GameManager.Instance.ClapperNumber = characterNumber;
        PlayerPrefs.SetInt("Character", characterNumber);
    }

    // Start is called before the first frame update
    void Start() {
        if (GameManager.Instance.ClapperNumber == characterNumber) {
            transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 0.65f);
        } else {
            transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
