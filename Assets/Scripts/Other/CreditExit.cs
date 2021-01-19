using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Common;

public class CreditExit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Inputmanager.GetKeyDownSubmit()) {
            SceneManager.LoadScene("Option");//もどる
        }
	}
}
