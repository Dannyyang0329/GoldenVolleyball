using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public string nextSceneName_1;
    public string nextSceneName_2;
    public string nextSceneName_3;

    public void LoadNextScene1() {
        StartCoroutine(LoadLevel(nextSceneName_1));
    }
    public void LoadNextScene2() {
        StartCoroutine(LoadLevel(nextSceneName_2));
    }
    public void LoadNextScene3() {
        StartCoroutine(LoadLevel(nextSceneName_3));
    }

    IEnumerator LoadLevel(string sceneName) {
        transition.SetTrigger("Start");
        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(sceneName);
    }
}
