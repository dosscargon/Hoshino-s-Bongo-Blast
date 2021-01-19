using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine;
using Assets.Scripts.Common;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

class GameManager {
    private GameManager() { }
    private static GameManager instance = new GameManager();
    public static GameManager Instance {
        get { return instance; }
    }
    public const string Version = "ver2.3.2";

    public readonly string[] clapperIDs = {
        "Hoshino",
        "Sukio",
        "Joushi",
        "Tsugurun",
        "KamuNi_Ai",
        "Tohu"
    };

    public bool IsAuto { get; set; } = false;
    /// <summary>
    /// プレイ中の曲情報
    /// </summary>
    public MusicData PlayingMusicData { get; private set; }
    /// <summary>
    /// スコア
    /// </summary>
    public int Score { get; set; }
    /// <summary>
    /// プレイ中の曲のハイスコア
    /// </summary>
    public int PlayingMusicHiScore { get; set; }
    /// <summary>
    /// プレイ中の曲のクリア情報
    /// </summary>
    public ClearData PlayingMusicClearData { get; set; }
    /// <summary>
    /// 難易度
    /// </summary>
    public int Difficulty { get; set; }

    private int clapperNumber;
    public int ClapperNumber {
        get {
            return clapperNumber;
        }
        set {
            if (value > clapperIDs.Count()) {
                clapperNumber = 0;
            } else {
                clapperNumber = value;
            }
        }
    }
    public string ClapperID {
        get {
            return clapperIDs[ClapperNumber];
        }
    }
    public CharacterData Clapper { get; set; }
    /// <summary>
    /// 通知数
    /// </summary>
    public float Buzz { get; set; }
    /// <summary>
    /// いくつ通知が来たらクリア？
    /// </summary>
    public const int buzzGoal = 10000;


    private int combo;
    /// <summary>
    /// コンボ数
    /// </summary>
    public int Combo {
        get => combo;
        set {
            combo = value;
            if (combo > MaxCombo) {
                MaxCombo = combo;
            }
        }
    }
    /// <summary>
    /// 最大コンボ数
    /// </summary>
    public int MaxCombo { get; private set; }

    /// <summary>
    /// 判定ごとのたたけた数
    /// </summary>
    private Dictionary<Judge, int> judgeCounter = new Dictionary<Judge, int>();
    /// <summary>
    /// たたけた数に+1
    /// </summary>
    public void JudgeCountPlus(Judge judge) {
        judgeCounter[judge]++;
    }
    /// <summary>
    /// たたけた数を取得
    /// </summary>
    public int getJudgeCount(Judge judge) {
        return judgeCounter[judge];
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    /// <param name="musicData"></param>
    public void StartGame(MusicData musicData) {
        PlayingMusicData = musicData;
        ResetData();

        //SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// データを消去してゲーム開始に備える
    /// </summary>
    private void ResetData() {
        Score = 0;
        Buzz = 0;
        Combo = 0;
        MaxCombo = 0;
        judgeCounter[Judge.Great] = 0;
        judgeCounter[Judge.Good] = 0;
        judgeCounter[Judge.Bad] = 0;
    }

    public int MasterVolume { get; set; } = 100;
    public int SEVolume { get; set; } = 100;
    public int KeyConfig { get; set; } = 0;

    public const float DefaultMusicVolume = 0.2f;
    public const float MaxSEVolume = 0.3f;

    private bool initialized = false;
    public void init() {
        if (!initialized) {

#if !UNITY_ANDROID
            Cursor.visible = false;
            Inputmanager.LoadConfig();
#endif
            MasterVolume = PlayerPrefs.GetInt("MusicVolume", 100);
            SEVolume = PlayerPrefs.GetInt("SEVolume", 100);
            KeyConfig = PlayerPrefs.GetInt("KeyConfig", 0);

            clapperNumber = PlayerPrefs.GetInt("Character", 0);

            //画面設定
            const string iniName = "config.ini";
            const string settingPattern = @".+?=.*?";
            string iniPath;
            int screenWidth = 1280;
            bool fullscreen = true;

            iniPath = Directory.GetParent(Application.dataPath).ToString() + Path.DirectorySeparatorChar + iniName;

            if (!File.Exists(iniPath)) {
            } else {
                string[] readText = File.ReadAllLines(iniPath);
                foreach (var text in readText) {
                    if (Regex.IsMatch(text, settingPattern)) {
                        int colonIndex = text.IndexOf('=');
                        string key = text.Substring(0, colonIndex);
                        string value = text.Substring(colonIndex + 1);

                        switch (key) {
                            case "ScreenWidth":
                                screenWidth = int.Parse(value);
                                break;
                            case "FullScreen":
                                fullscreen = (value == "1");
                                break;
                        }
                    }
                }
            }
            Screen.SetResolution(screenWidth, screenWidth / 16 * 9, fullscreen);

            initialized = true;
        }
    }
}
