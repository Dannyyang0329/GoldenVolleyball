using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static bool isStart = false;

    public GameObject multiPlayerUI;

    public Animator roundCamAnim;
    public CinemachineVirtualCamera roundCam;

    public string ipAddress = "127.0.0.1";
    private UNetTransport transport;

    private void Start() {
        multiPlayerUI.SetActive(true);
    }

    public void Game_Host() {
        transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = ipAddress;

        NetworkManager.Singleton.StartHost();
        isStart = true;
        //GetComponent<NetworkSpawner>().SpawnPlayer();

        multiPlayerUI.SetActive(false);

        roundCamAnim.SetTrigger("Start");
    }

    public void Game_Client() {
        transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = ipAddress;

        NetworkManager.Singleton.StartClient();
        isStart = true;
        //GetComponent<NetworkSpawner>().SpawnPlayer();

        multiPlayerUI.SetActive(false);

        roundCamAnim.SetTrigger("Start");
    }

    public void SetIpAddress(string newAddress) {
        this.ipAddress = newAddress;
    }

    private void Update() {
        if(roundCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition >= 20) {
            roundCam.Priority = 0;
        }      
    }

}
