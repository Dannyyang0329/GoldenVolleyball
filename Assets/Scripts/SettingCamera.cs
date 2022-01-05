using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MLAPI;

public class SettingCamera : NetworkBehaviour
{
    public CinemachineVirtualCamera theCamera;

    private void Start() {
        if (IsLocalPlayer) {
            theCamera.Priority += 1;
        }
    }
}
