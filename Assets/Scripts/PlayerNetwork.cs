using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class PlayerNetwork : NetworkBehaviour 
{
    public NetworkVariableVector2 Position = new NetworkVariableVector2(new NetworkVariableSettings {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    public override void NetworkStart() {
        Move();
    }

    public void Move() {
        if (NetworkManager.Singleton.IsServer) {
            var randomPosition = GetRandomPosition();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }
        else SubmitPositionRequestServerRPC();
    }

    [ServerRpc]
    void SubmitPositionRequestServerRPC(ServerRpcParams rpcParams = default) {
        Position.Value = GetRandomPosition();
    }
    static Vector2 GetRandomPosition() {
        return new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
    }

    private void Update() {
        transform.position = Position.Value;
    }
}
