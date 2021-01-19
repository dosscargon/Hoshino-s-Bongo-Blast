using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Unlocker : MonoBehaviour {
    abstract public bool CheckUnlockCleared();

    // Start is called before the first frame update
    void Start() {
        if (CheckUnlockCleared()) {
            transform.GetChild(2).gameObject.SetActive(false);
        } else {
            transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
