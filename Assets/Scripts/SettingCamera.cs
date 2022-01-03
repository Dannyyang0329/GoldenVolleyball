using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MLAPI;

public class SettingCamera : NetworkBehaviour
{
    public CinemachineVirtualCamera camera;

    private void Start() {
        if (IsLocalPlayer) {
            camera.Priority += 1;
        }
    }
}
