using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public AudioManager audioManager;

    private void Start() {
        audioManager.Play("Bgm");
    }

    public void Quit() {
        Application.Quit();
    }

    public void HoverSound() {
        audioManager.Play("HoverSound");
    }
}
