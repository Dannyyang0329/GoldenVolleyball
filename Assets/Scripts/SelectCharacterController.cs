using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class SelectCharacterController : NetworkBehaviour
{
    public static NetworkVariableString selectedPlayer = new NetworkVariableString("Mario");

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

        selectedPlayer.Value = "Luigi";
    }

    public void PlayMario() {
        Quiet();
        audioManager.Play("Mario_Sound");
        marioAnim.SetTrigger("Start");

        selectedPlayer.Value = "Mario";
    }

    public void PlayToad() {
        Quiet();
        audioManager.Play("Toad_Sound");
        toadAnim.SetTrigger("Start");

        selectedPlayer.Value = "Toad";
    }

    private void Quiet() {
        audioManager.Stop("Mario_Sound");
        audioManager.Stop("Luigi_Sound");
        audioManager.Stop("Toad_Sound");
    }
}
