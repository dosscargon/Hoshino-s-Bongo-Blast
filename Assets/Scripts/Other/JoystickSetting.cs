using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common;
using UnityEngine.SceneManagement;

class JoystickSetting : MonoBehaviour {
    [SerializeField]
    private Image joystickSettingImage;
    [SerializeField]
    private Text joystickSettingText;

    [SerializeField]
    private Image joystickImage;
    [SerializeField]
    private List<Sprite> joystickSettingSplites;

    private void Start() {
        StartCoroutine("SetJoystickButton");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("KeyConfig");
        }
    }

    public IEnumerator SetJoystickButton() {
        joystickSettingImage.gameObject.SetActive(true);

        for (int i = 0; i < 9; i++) {
            joystickImage.sprite = joystickSettingSplites[i];
            for (; ; ) {
                if (i <= 3 || i == 8) {
                    KeyCode button = Inputmanager.GetButtonNumer();
                    if (button != KeyCode.None) {
                        switch (i) {
                            case 0:
                                Inputmanager.ButtonA = button;
                                break;
                            case 1:
                                Inputmanager.ButtonB = button;
                                break;
                            case 2:
                                Inputmanager.ButtonX = button;
                                break;
                            case 3:
                                Inputmanager.ButtonY = button;
                                break;
                            case 8:
                                Inputmanager.ButtonStart = button;
                                break;
                        }
                        yield return null;
                        break;
                    }
                } else {
                    (int, int) axis = Inputmanager.GetAxisNumber();
                    if (axis.Item1 != -1) {
                        switch (i) {
                            case 4:
                                Inputmanager.upKey = (axis.Item1 + 1, axis.Item2);
                                break;
                            case 5:
                                Inputmanager.downKey = (axis.Item1 + 1, axis.Item2);
                                break;
                            case 6:
                                Inputmanager.rightKey = (axis.Item1 + 1, axis.Item2);
                                break;
                            case 7:
                                Inputmanager.leftKey = (axis.Item1 + 1, axis.Item2);
                                break;
                        }
                        yield return null;
                        break;
                    }
                }
                yield return null;
            }
        }
        Inputmanager.SaveConfig();
        SceneManager.LoadScene("KeyConfig");
    }

}
