using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour {
    /// <summary>
    /// シーンを読み込む
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    public void LoadScene(string sceneName) {
        GameObject.Find("SceneController").GetComponent<Animator>().SetTrigger("MoveScene");

        //GameObject.Find("Canvas").transform.Find("Loading").gameObject.SetActive(true);
        StartCoroutine("moveScene",sceneName);
        //SceneManager.LoadScene(sceneName);
    }

    IEnumerator moveScene(string sceneName) {
        yield return new WaitForSeconds(0.40f);

        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneWithoutAnimation(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    //IEnumerator loading(string sceneName) {
    //    var async = SceneManager.LoadSceneAsync(sceneName);
    //    async.allowSceneActivation = false;
    //    //SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
    //    while (async.progress < 0.9f) {
    //        yield return null;
    //    }
    //    async.allowSceneActivation = true;
    //    //SceneManager.UnloadSceneAsync("Loading");
    //}
}
