using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common {
    class Inputmanager {
        static public KeyCode ButtonA { get; set; }
        static public KeyCode ButtonB { get; set; }
        static public KeyCode ButtonX { get; set; }
        static public KeyCode ButtonY { get; set; }
        static public KeyCode ButtonStart { get; set; }

        static public (int, int) upKey { get; set; }
        static public (int, int) downKey { get; set; }
        static public (int, int) leftKey { get; set; }
        static public (int, int) rightKey { get; set; }

        static private bool isUpLocked = false;
        static private bool isDownLocked = false;
        static private bool isLeftLocked = false;
        static private bool isRightLocked = false;

        static private List<bool> axisLock = Enumerable.Repeat(false, 28).ToList();

        static public bool GetKeyDownSubmit() {
            return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(ButtonA) || Input.GetKeyDown(ButtonStart);
        }
        static public bool GetKeyDownD() {
            return Input.GetKeyDown(KeyCode.D) || GetDpadDownLeft();
        }
        static public bool GetKeyDownF() {
            return Input.GetKeyDown(KeyCode.F) || GetDpadDownRight();
        }
        static public bool GetKeyDownJ() {
            return Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(ButtonY);
        }
        static public bool GetKeyDownK() {
            return Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(ButtonA);
        }

        static public bool GetKeyDownQuit() {
            return Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(ButtonStart);
        }

        static public bool GetDpadDownUp() {
            if ((Input.GetAxisRaw($"axis{upKey.Item1}") == upKey.Item2 && !isUpLocked)) {
                isUpLocked = true;
                return true;
            }
            if (Input.GetAxisRaw($"axis{upKey.Item1}") != upKey.Item2) {
                isUpLocked = false;
            }
            return false;
        }

        static public bool GetKeyDownUpArrow() {
            if (Input.GetKeyDown(KeyCode.UpArrow) || GetDpadDownUp()) {
                return true;
            }

            return false;
        }

        static public bool GetDpadDownDown() {
            if ((Input.GetAxisRaw($"axis{downKey.Item1}") == downKey.Item2 && !isDownLocked)) {
                isDownLocked = true;
                return true;
            }
            if (Input.GetAxisRaw($"axis{downKey.Item1}") != downKey.Item2) {
                isDownLocked = false;
            }
            return false;
        }
        static public bool GetKeyDownDownArrow() {
            if (Input.GetKeyDown(KeyCode.DownArrow) || GetDpadDownDown()) {
                return true;
            }
            return false;
        }

        static public bool GetDpadDownLeft() {
            if ((Input.GetAxisRaw($"axis{leftKey.Item1}") == leftKey.Item2 && !isLeftLocked)) {
                isLeftLocked = true;
                return true;
            }
            if (Input.GetAxisRaw($"axis{leftKey.Item1}") != leftKey.Item2) {
                isLeftLocked = false;
            }
            return false;
        }
        static public bool GetKeyDownLeftArrow() {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || GetDpadDownLeft()) {
                return true;
            }
            return false;
        }
        static public bool GetDpadDownRight() {
            if ((Input.GetAxisRaw($"axis{rightKey.Item1}") == rightKey.Item2 && !isRightLocked)) {
                isRightLocked = true;
                return true;
            }
            if (Input.GetAxisRaw($"axis{rightKey.Item1}") != rightKey.Item2) {
                isRightLocked = false;
            }
            return false;
        }
        static public bool GetKeyDownRightArrow() {
            if (Input.GetKeyDown(KeyCode.RightArrow) || GetDpadDownRight()) {
                return true;
            }
            return false;
        }

        static public bool GetKeyUp() {
            return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxisRaw($"axis{upKey.Item1}") == upKey.Item2;
        }
        static public bool GetKeyDown() {
            return Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxisRaw($"axis{downKey.Item1}") == downKey.Item2;
        }
        static public bool GetKeyLeft() {
            return Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxisRaw($"axis{leftKey.Item1}") == leftKey.Item2;
        }
        static public bool GetKeyRight() {
            return Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxisRaw($"axis{rightKey.Item1}") == rightKey.Item2;
        }

        static public KeyCode GetButtonNumer() {
            for(var key = KeyCode.JoystickButton0; key <= KeyCode.JoystickButton19; key++) {
                if (Input.GetKeyDown(key)) {
                    return key;
                }
            }
            return KeyCode.None;
        }
        static public (int,int) GetAxisNumber() {
            (int, int) activeAxis = (-1, 0);
            for(int i = 0; i < 28; i++) {
                //if(Input.GetKeyDown($"joystick axis {i}")) {
                //    Debug.Log($"{i} pushed");
                //}
                int axisRaw;
                if (!axisLock[i]) {
                    if (Mathf.Abs(axisRaw = (int)Input.GetAxisRaw($"axis{i + 1}")) == 1) {
                        activeAxis = (i, axisRaw);
                        axisLock[i] = true;
                        break;
                    }
                }
            }

            for (int i = 0; i < 28; i++) {
                if (i != activeAxis.Item1) {
                    if (Mathf.Abs(Input.GetAxisRaw($"axis{i + 1}")) <= 0.1) {
                        axisLock[i] = false;
                    }
                }
            }

            return activeAxis;
        }

        static public void SaveConfig() {
            PlayerPrefs.SetInt("JoystickA", (int)ButtonA);
            PlayerPrefs.SetInt("JoystickB", (int)ButtonB);
            PlayerPrefs.SetInt("JoystickX", (int)ButtonX);
            PlayerPrefs.SetInt("JoystickY", (int)ButtonY);

            PlayerPrefs.SetInt("JoystickUp_Axis", upKey.Item1);
            PlayerPrefs.SetInt("JoystickUp_Direction", upKey.Item2);
            PlayerPrefs.SetInt("JoystickDown_Axis", downKey.Item1);
            PlayerPrefs.SetInt("JoystickDown_Direction", downKey.Item2);
            PlayerPrefs.SetInt("JoystickLeft_Axis", leftKey.Item1);
            PlayerPrefs.SetInt("JoystickLeft_Direction", leftKey.Item2);
            PlayerPrefs.SetInt("JoystickRight_Axis", rightKey.Item1);
            PlayerPrefs.SetInt("JoystickRight_Direction", rightKey.Item2);

            PlayerPrefs.GetInt("JoystickStart", (int)ButtonStart);
        }

        static public void LoadConfig() {
            ButtonA = (KeyCode)PlayerPrefs.GetInt("JoystickA", (int)KeyCode.JoystickButton1);
            ButtonB = (KeyCode)PlayerPrefs.GetInt("JoystickB",  (int)KeyCode.JoystickButton0);
            ButtonX = (KeyCode)PlayerPrefs.GetInt("JoystickX",  (int)KeyCode.JoystickButton3);
            ButtonY = (KeyCode)PlayerPrefs.GetInt("JoystickY", (int)KeyCode.JoystickButton2);

            (int, int) key;
            key.Item1 = PlayerPrefs.GetInt("JoystickUp_Axis", 7);
            key.Item2=PlayerPrefs.GetInt("JoystickUp_Direction", 1);
            upKey = key;
            key.Item1=PlayerPrefs.GetInt("JoystickDown_Axis", 7);
            key.Item2=PlayerPrefs.GetInt("JoystickDown_Direction", -1);
            downKey = key;
            key.Item1=PlayerPrefs.GetInt("JoystickLeft_Axis", 6);
            key.Item2=PlayerPrefs.GetInt("JoystickLeft_Direction", -1);
            leftKey = key;
            key.Item1=PlayerPrefs.GetInt("JoystickRight_Axis", 6);
            key.Item2=PlayerPrefs.GetInt("JoystickRight_Direction", 1);
            rightKey = key;

            PlayerPrefs.GetInt("JoystickStart", (int)ButtonStart);
        }
    }
}
