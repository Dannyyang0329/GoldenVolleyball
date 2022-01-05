using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class NetworkSpawner : NetworkBehaviour
{
    public Vector3[] spawnPoint;

    public GameObject[] prefabs;

    public void SpawnPlayer() 
    {
        ulong id = NetworkManager.Singleton.LocalClientId;
        Debug.Log(id);

        if (IsHost) {
            GameObject obj = Instantiate(prefabs[3], spawnPoint[4], Quaternion.identity).gameObject;
            obj.GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.Singleton.LocalClientId, null, true);

            Spawn(id);
        }
        else if (NetworkManager.Singleton.IsClient) {
            SpawnServerRpc(id);
        }

    }

    [ServerRpc]
    void SpawnServerRpc(ulong clientId) {
        Spawn(clientId);
    }

    public void Spawn(ulong id) {
        GameObject obj;

        if(SelectCharacterController.selectedPlayer == "Mario") {
            obj = Instantiate(prefabs[0], spawnPoint[0], Quaternion.identity).gameObject;
            obj.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, null, true);
        }
        if(SelectCharacterController.selectedPlayer == "Luigi") {
            obj = Instantiate(prefabs[1], spawnPoint[1], Quaternion.identity).gameObject;
            obj.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, null, true);
        }
        if(SelectCharacterController.selectedPlayer == "Toad") {
            obj = Instantiate(prefabs[2], spawnPoint[2], Quaternion.identity).gameObject;
            obj.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, null, true);
        }
    }
}
