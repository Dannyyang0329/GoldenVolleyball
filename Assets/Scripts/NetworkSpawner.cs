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

        if (NetworkManager.Singleton.IsServer) {
            Debug.Log("Server");

            GameObject obj = Instantiate(prefabs[3], spawnPoint[4], Quaternion.identity).gameObject;
            obj.GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.Singleton.LocalClientId, null, true);

            Spawning(id);
        }
        else{
            Debug.Log("Client");

            SpawningServerRpc();
        }

    }

    [ServerRpc]
    void SpawningServerRpc(ServerRpcParams rpcParams = default) {
        Debug.Log("SpawningServerRpc!");
        //SpawningClientRpc(id);
    }

    [ClientRpc]
    void SpawningClientRpc(ulong id) {
        Debug.Log("SpawningClientRpc!");
        Spawning(id);
    }

    void Spawning(ulong id) {
        GameObject obj = null;

        if(SelectCharacterController.selectedPlayer == "Mario") {
            obj = Instantiate(prefabs[0], spawnPoint[0], Quaternion.identity).gameObject;
            obj.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, null, true);
            //obj.GetComponent<NetworkObject>().SpawnWithOwnership(id, null, true);
        }
        else if(SelectCharacterController.selectedPlayer == "Luigi") {
            obj = Instantiate(prefabs[1], spawnPoint[1], Quaternion.identity).gameObject;
            obj.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, null, true);
            //obj.GetComponent<NetworkObject>().SpawnWithOwnership(id, null, true);
        }
        else if(SelectCharacterController.selectedPlayer == "Toad") {
            obj = Instantiate(prefabs[2], spawnPoint[2], Quaternion.identity).gameObject;
            obj.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, null, true);
            //obj.GetComponent<NetworkObject>().SpawnWithOwnership(id, null, true);
        }
    }
}
