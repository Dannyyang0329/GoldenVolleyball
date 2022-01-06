using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class NetworkSpawner : NetworkBehaviour
{
    public Vector3[] spawnPoint;

    public NetworkObject[] prefabs;

    public NetworkVariableInt playerIndex;

    private void Start() {
        if(IsLocalPlayer) {
            SpawnPlayer();
            Destroy(gameObject);
        }
    }

    public void SpawnPlayer() 
    {
        ulong id = NetworkManager.Singleton.LocalClientId;

        if (NetworkManager.Singleton.IsServer) {
            Debug.Log("Server : " + SelectCharacterController.selectedPlayer.Value);

            NetworkObject obj = Instantiate(prefabs[3], spawnPoint[4], Quaternion.identity);
            obj.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);

            Spawning(id, SelectCharacterController.selectedPlayer.Value);
        }
        else{
            Debug.Log("Client : " + SelectCharacterController.selectedPlayer.Value);

            SpawningServerRpc(id, SelectCharacterController.selectedPlayer.Value);
        }
    }

    [ServerRpc]
    void SpawningServerRpc(ulong id, string characterStr) {
        Spawning(id, characterStr);
    }


    void Spawning(ulong id, string name) {
        NetworkObject obj = null;

        if(name == "Mario") {
            obj = Instantiate(prefabs[0], spawnPoint[0], Quaternion.identity);
            obj.SpawnAsPlayerObject(id);
        }
        else if(name == "Luigi") {
            obj = Instantiate(prefabs[1], spawnPoint[1], Quaternion.identity);
            obj.SpawnAsPlayerObject(id);
        }
        else if(name == "Toad") {
            obj = Instantiate(prefabs[2], spawnPoint[2], Quaternion.identity);
            obj.SpawnAsPlayerObject(id);
        }
    }

    int GetPlayerCount() {
        // not finish
        return 0; 
    }
}
