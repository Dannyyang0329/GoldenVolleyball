using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public GameObject multiPlayerUI;

    public Animator roundCamAnim;
    public CinemachineVirtualCamera roundCam;

    public string ipAddress = "127.0.0.1";
    private UNetTransport transport;

    public static int curViewTeam;

    private void Start() {

        GameObject.FindObjectOfType<AudioManager>().Play("Bgm");
        multiPlayerUI.SetActive(true);
    }

    public void Game_Host() {
        transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = ipAddress;

        NetworkManager.Singleton.StartHost();

        multiPlayerUI.SetActive(false);

        roundCamAnim.SetTrigger("Start");
    }

    public void Game_Client() {
        transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = ipAddress;

        NetworkManager.Singleton.StartClient();

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
