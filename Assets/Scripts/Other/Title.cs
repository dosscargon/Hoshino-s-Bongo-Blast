using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour {
    [SerializeField] Text versionText;
    private void Awake() {
        GameManager.Instance.init();
    }

    // Start is called before the first frame update
    void Start() {
        versionText.text = GameManager.Version;
    }

    // Update is called once per frame
    void Update() {

    }
}
