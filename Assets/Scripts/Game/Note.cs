using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Note : MonoBehaviour {
    #region UI
    public GameObject LaneManagerGameobject;
    #endregion
    /// <summary>
    /// ノートの種類
    /// </summary>
    public NoteType noteType;

    /// <summary>
    /// 出現するタイミング
    /// </summary>
    public float appearanceTime;
    /// <summary>
    /// いつ叩かれるべきか
    /// </summary>
    public float targetTime;

    const float DefaultAppearancePositionX = 500;

    private float speed = 1;
    /// <summary>
    /// 最初どこに出現するか
    /// </summary>
    public float Speed {
        get => speed;
        set {
            if (value != 0) {
                speed = value;
            }
        }
    }
    /// <summary>
    /// 何拍前から動き始めるか
    /// </summary>
    private float AppearanceBeat {
        get {
            if (Speed < 1) {
                return 1 / Speed;
            } else {
                return 1;
            }
        }
    }

    private RectTransform rTransform;

    /// <summary>
    /// ノーツ管理者
    /// </summary>
    private LaneController owner;

    /// <summary>
    /// 叩かれずにBad判定を受けたか
    /// </summary>
    private bool threwFlag = false;

    // Use this for initialization
    void Start () {
        rTransform = GetComponent<RectTransform>();
        owner = LaneManagerGameobject.GetComponent<LaneController>();
        //speed *= -1;
	}
	
	// Update is called once per frame
	void Update () {
        //曲の進行に合わせて動く
        if (owner.AudioTime > appearanceTime - owner.BeatLength * owner.Measure * AppearanceBeat) {
            rTransform.localPosition = new Vector3(rTransform.localPosition.x,
                Speed * DefaultAppearancePositionX * ((targetTime - owner.AudioTime) / (targetTime - appearanceTime)), targetTime);


            if ((owner.AudioTime - targetTime) >= owner.JudgeRangeBad && !threwFlag && noteType != NoteType.Bar) {
                owner.AddScore(Judge.Bad);
                threwFlag = true;
            }

            //画面外に出たら消える
            if (rTransform.localPosition.y < -120) {
                owner.DestroyNotes(this);
            }

            //オート
            if (GameManager.Instance.IsAuto) {
                if (targetTime - owner.AudioTime <= 0) {
                    if (noteType == NoteType.Like) {
                        owner.ClapRight();
                    } else if (noteType == NoteType.RT) {
                        owner.ClapLeft();
                    }
                }
            }
        }
	}
}
