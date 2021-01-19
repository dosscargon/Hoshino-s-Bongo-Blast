using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Common;

public class MenuButton : MonoBehaviour {
    [Serializable]
    private class Destination {
        [SerializeField]
        public MenuButton Up;
        [SerializeField]
        public MenuButton Down;
        [SerializeField]
        public MenuButton Left;
        [SerializeField]
        public MenuButton Right;
    }

    /// <summary>
    /// 十字キーを押した際，どこにフォーカスを移動するか
    /// </summary>
    [SerializeField]
    private Destination destination;
    /// <summary>
    /// 決定キーを押したら発火するイベント
    /// </summary>
    [SerializeField]
    private UnityEvent OkEvents;

    [SerializeField]
    private UnityEvent FocusedEvents;

    //移動キーを押したら発火するイベント
    [SerializeField]
    private UnityEvent UpEvents;
    [SerializeField]
    private UnityEvent DownEvents;
    [SerializeField]
    private UnityEvent LeftEvents;
    [SerializeField]
    private UnityEvent RightEvents;

    private static MenuButton focusedComponent;
    /// <summary>
    /// フォーカスされてるか
    /// </summary>
    protected bool Focus { get; private set; }
    /// <summary>
    /// フォーカスされてるボタン．フォーカスされるのは一つだけ
    /// </summary>
    public static MenuButton FocusedComponent {
        get => focusedComponent;
        set {
            if (value != null) {
                if (FocusedComponent != null) {
                    focusedComponent.Focus = false;
                    focusedComponent.GetComponent<Animator>().SetBool("Focus", focusedComponent.Focus);
                }
                focusedComponent = value;
                focusedComponent.Focus = true;
                focusedComponent.GetComponent<Animator>().SetBool("Focus", focusedComponent.Focus);

                focusTime = Time.time;
                FocusedComponent.FocusedEvents.Invoke();
            }
        }
    }

    private static float focusTime;
    private const float cooldownTime = 0;

    [SerializeField]
    private bool allowFirstFocus;

    // Use this for initialization
    void Start() {
        FirstFocus();
    }

    /// <summary>
    /// 誰もフォーカスされてなければフォーカスする
    /// </summary>
    protected void FirstFocus() {
        if (FocusedComponent == null && allowFirstFocus) {
            FocusedComponent = this;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Time.time - focusTime > cooldownTime) {
            Inputkey();
        }
    }

    /// <summary>
    /// 押したキーに応じてフォーカス移動したりイベント発火したり
    /// </summary>
    protected void Inputkey() {
        if (Focus) {
            if (Inputmanager.GetKeyDownSubmit()) {
                OkEvents.Invoke();
            } else if (Inputmanager.GetKeyDownUpArrow()) {
                MoveUp();
            } else if (Inputmanager.GetKeyDownDownArrow()) {
                MoveDown();
            } else if (Inputmanager.GetKeyDownLeftArrow()) {
                MoveLeft();
            } else if (Inputmanager.GetKeyDownRightArrow()) {
                MoveRight();
            }
        }
    }

    public void MoveUp() {
        FocusedComponent = destination.Up;
    }
    public void MoveDown() {
        FocusedComponent = destination.Down;
    }
    public void MoveLeft() {
        FocusedComponent = destination.Left;
    }
    public void MoveRight() {
        FocusedComponent = destination.Right;
    }

#if UNITY_ANDROID
    public void OnButtonClick() {
        if (!Focus) {
            focusedComponent = this;
        } else {
            OkEvents.Invoke();
        }
    }
#endif
}
