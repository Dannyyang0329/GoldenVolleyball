using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using MLAPI;

public class GameManager : NetworkBehaviour
{
    public GameObject multiPlayerUI;

    public Animator roundCamAnim;
    public CinemachineVirtualCamera roundCam;

    private void Start() {
        multiPlayerUI.SetActive(true);
    }

    public void Game_Host() {
        NetworkManager.Singleton.StartHost();

        multiPlayerUI.SetActive(false);

        roundCamAnim.SetTrigger("Start");
    }

    public void Game_Client() {
        NetworkManager.Singleton.StartClient();

        multiPlayerUI.SetActive(false);

        roundCamAnim.SetTrigger("Start");
    }

    private void Update() {
        if(roundCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition >= 20) {
            roundCam.Priority = 0;
        }      

        // if(NetworkManager.)
    }
}
