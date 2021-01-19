using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

public class SNSAgeIkou : MonoBehaviour {
    private int counter = 0;
    [SerializeField]
    private Text memo2;
    public void ikou() {
        if (counter == 0) {
            memo2.text = "確認のため\nもう一度\n押して下さい";
            counter = 1;

        } else if (counter == 1) {
            PlayerPrefs.DeleteKey("snsage_hard.HiScore");
            PlayerPrefs.DeleteKey("snsage_hard.ClearData");

            memo2.text = "完了";
            counter = 2;
        }
    }
}
