using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterController : MonoBehaviour
{
    public static string selectedPlayer;

    public AudioManager audioManager;
    public Animator marioAnim;
    public Animator luigiAnim;
    public Animator toadAnim;

    private void Start() {
        audioManager.Play("Bgm");
    }

    public void PlayLuigi() {
        Quiet();
        audioManager.Play("Luigi_Sound");
        luigiAnim.SetTrigger("Start");

        selectedPlayer = "Luigi";
    }

    public void PlayMario() {
        Quiet();
        audioManager.Play("Mario_Sound");
        marioAnim.SetTrigger("Start");

        selectedPlayer = "Mario";
    }

    public void PlayToad() {
        Quiet();
        audioManager.Play("Toad_Sound");
        toadAnim.SetTrigger("Start");

        selectedPlayer = "Toad";
    }

    private void Quiet() {
        audioManager.Stop("Mario_Sound");
        audioManager.Stop("Luigi_Sound");
        audioManager.Stop("Toad_Sound");
    }
}
