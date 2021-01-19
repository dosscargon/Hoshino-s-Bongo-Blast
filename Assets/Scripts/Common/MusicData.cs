using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 曲のタイトル，譜面データ，音楽ファイルなどのデータ群
/// </summary>
[Serializable]
public class MusicData {
    [SerializeField]
    private string title;
    /// <summary>
    /// 曲名
    /// </summary>
    public string Title {
        get { return title; }
    }

    [SerializeField]
    [Multiline]
    private string memo;
    /// <summary>
    /// 曲の情報．権利者とか出典とか
    /// </summary>
    public string Memo {
        get { return memo; }
    }

    [SerializeField]
    private Sprite icon;
    /// <summary>
    /// メニューに表示されるアイコン
    /// </summary>
    public Sprite Icon {
        get { return icon; }
    }

    [SerializeField]
    private AudioClip audio;
    /// <summary>
    /// 音楽ファイル
    /// </summary>
    public AudioClip Clip {
        get { return audio; }
    }

    [SerializeField]
    private float demoStart;
    /// <summary>
    /// 曲選択画面のデモをどこから流すか
    /// </summary>
    public float DemoStart {
        get { return demoStart; }
    }

    [SerializeField]
    private TextAsset txt;
    /// <summary>
    /// 譜面データ
    /// </summary>
    public TextAsset Txt {
        get { return txt; }
    }

    [SerializeField]
    private string musicID;
    /// <summary>
    /// 曲ID．セーブデータの管理に使う
    /// </summary>
    public string MusicID {
        get { return musicID; }
    }

    [SerializeField]
    private float volume = 0;
    /// <summary>
    /// 曲の個別ボリューム．0にするとデフォルト値になる
    /// </summary>
    public float Volume {
        get {
            if (volume <= 0) {
                return GameManager.DefaultMusicVolume;
            } else {
                return volume;
            }
        }
        set {
            volume = value;
        }
    }
}
