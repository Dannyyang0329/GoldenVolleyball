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

    public int playerNumber;

    private void Start() {
        if(IsLocalPlayer) {
            SpawnPlayer();
            //Destroy(gameObject);
        }
    }

    public void SpawnPlayer() 
    {
        ulong id = NetworkManager.Singleton.LocalClientId;
        playerNumber = (int) id / 2;

        if (NetworkManager.Singleton.IsServer) {
            // Spawn ball
            NetworkObject obj = Instantiate(prefabs[6], spawnPoint[4], Quaternion.identity);
            obj.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);

            // Spawn player
            Spawning(id, SelectCharacterController.selectedPlayer.Value, playerNumber);

            Debug.Log("Server : " + SelectCharacterController.selectedPlayer.Value);
        }
        else{
            // Spawn player
            SpawningServerRpc(id, SelectCharacterController.selectedPlayer.Value, playerNumber);

            Debug.Log("Client : " + SelectCharacterController.selectedPlayer.Value);
        }
    }

    [ServerRpc]
    void SpawningServerRpc(ulong id, string characterStr, int point) {
        Spawning(id, characterStr, point);
    }


    void Spawning(ulong id, string name, int point) {
        NetworkObject obj = null;
        Debug.Log(point);

        bool isRev = (point % 2 == 1);

        if(name == "Mario") {
            if(!isRev) obj = Instantiate(prefabs[0], spawnPoint[point], Quaternion.identity);
            else obj = Instantiate(prefabs[3], spawnPoint[point], Quaternion.identity);

            obj.SpawnAsPlayerObject(id);
        }
        else if(name == "Luigi") {
            if(!isRev) obj = Instantiate(prefabs[1], spawnPoint[point], Quaternion.identity);
            else obj = Instantiate(prefabs[4], spawnPoint[point], Quaternion.identity);

            obj.SpawnAsPlayerObject(id);
        }
        else if(name == "Toad") {
            if(!isRev) obj = Instantiate(prefabs[2], spawnPoint[point], Quaternion.identity);
            else obj = Instantiate (prefabs[5], spawnPoint[point], Quaternion.identity);

            obj.SpawnAsPlayerObject(id);
        }
    }
}
