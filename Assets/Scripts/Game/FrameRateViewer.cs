using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameRateViewer : MonoBehaviour {

    // 変数
    int frameCount;
    float prevTime;
    float fps;

    public Text text;

    // 初期化処理
    void Start() {
        frameCount = 0;
        prevTime = 0.0f;
    }
    // 更新処理
    void Update() {
        frameCount++;
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f) {
            fps = frameCount / time;
            text.text = $"fps={fps}";

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }
    }
}