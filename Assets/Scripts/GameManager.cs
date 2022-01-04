using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class GameManager : NetworkBehaviour
{
    public GameObject singlePlayerUI;
    public GameObject multiPlayerUI;

    private void Start() {
        if (MainMenuController.isSingle) singlePlayerUI.SetActive(true);
        else multiPlayerUI.SetActive(true);
    }

    public void Game_Host() {
        NetworkManager.Singleton.StartHost();

        if (MainMenuController.isSingle) singlePlayerUI.SetActive(false);
        else multiPlayerUI.SetActive(false);
    }

    public void Game_Client() {
        NetworkManager.Singleton.StartClient();

        if (MainMenuController.isSingle) singlePlayerUI.SetActive(false);
        else multiPlayerUI.SetActive(false);
    }
}
