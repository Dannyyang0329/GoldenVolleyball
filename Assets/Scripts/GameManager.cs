using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static int curViewTeam;
    
    public GameObject multiPlayerUI;

    public Animator roundCamAnim;
    public CinemachineVirtualCamera roundCam;

    private UNetTransport transport;
    public string ipAddress = "127.0.0.1";

    private void Start() 
    {
        GameObject.FindObjectOfType<AudioManager>().Play("Bgm");
        multiPlayerUI.SetActive(true);

        Invoke("SwitchCam", 10);
    }

    public void Game_Host() 
    {
        transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = ipAddress;

        NetworkManager.Singleton.StartHost();

        multiPlayerUI.SetActive(false);

        roundCamAnim.SetTrigger("Start");
    }

    public void Game_Client() 
    {
        transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = ipAddress;

        NetworkManager.Singleton.StartClient();

        multiPlayerUI.SetActive(false);

        roundCamAnim.SetTrigger("Start");
    }

    public void SetIpAddress(string newAddress) 
    {
        this.ipAddress = newAddress;
    }

    private void SwitchCam() {
        roundCam.Priority = 0;
    }

    public void loadScene_meun() {
        Disconnect();
        GetComponent<LevelLoader>().LoadNextScene2();
    }

    public void loadScene_restart() {
        Disconnect();
        GetComponent<LevelLoader>().LoadNextScene1();
    }

    public void Disconnect() {
        if (IsHost) {
            NetworkManager.Singleton.StopHost();
        }
        else if (IsClient) {
            NetworkManager.Singleton.StopClient();
        }
        else if (IsServer) {
            NetworkManager.Singleton.StopServer();
        }
    }
}
