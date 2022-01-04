using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public static bool isSingle = false;

    public AudioManager audioManager;

    private void Start() {
        audioManager.Play("Bgm");
    }

    public void Quit() {
        Application.Quit();
    }

    public void SinglePlayer() {
        GetComponent<LevelLoader>().LoadNextScene1();
        isSingle = true;
    }

    public void MultiPlayer() {
        GetComponent<LevelLoader>().LoadNextScene1();
        isSingle = false;
    }

    public void HoverSound() {
        audioManager.Play("HoverSound");
    }
}
