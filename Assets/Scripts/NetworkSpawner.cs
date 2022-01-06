using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class NetworkSpawner : NetworkBehaviour
{
    public Vector3[] spawnPoint;

    public NetworkObject[] prefabs;

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
            Debug.Log("Server");

            NetworkObject obj = Instantiate(prefabs[3], spawnPoint[4], Quaternion.identity);
            obj.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);

            Spawning(id);
        }
        else{
            Debug.Log("Client");

            //SpawningClientRpc(id);
            SpawningServerRpc(id);
        }
    }

    [ServerRpc]
    void SpawningServerRpc(ulong id) {
        Debug.Log("Spawn");
        Spawning(id);
        //SpawningClientRpc(id);
    }

    [ClientRpc]
    void SpawningClientRpc(ulong id) {
        Debug.Log("SpawningClientRpc!");
        Spawning(id);
    }

    void Spawning(ulong id) {
        NetworkObject obj = null;

        if(SelectCharacterController.selectedPlayer == "Mario") {
            obj = Instantiate(prefabs[0], spawnPoint[0], Quaternion.identity);
            obj.SpawnAsPlayerObject(id);
            //obj.GetComponent<NetworkObject>().SpawnWithOwnership(id, null, true);
        }
        else if(SelectCharacterController.selectedPlayer == "Luigi") {
            obj = Instantiate(prefabs[1], spawnPoint[1], Quaternion.identity);
            obj.SpawnAsPlayerObject(id);
            //obj.GetComponent<NetworkObject>().SpawnWithOwnership(id, null, true);
        }
        else if(SelectCharacterController.selectedPlayer == "Toad") {
            obj = Instantiate(prefabs[2], spawnPoint[2], Quaternion.identity);
            obj.SpawnAsPlayerObject(id);
            //obj.GetComponent<NetworkObject>().SpawnWithOwnership(id, null, true);
        }
    }
}
