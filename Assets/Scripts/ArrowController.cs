using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Transform ball;

    void Update()
    {
        Vector3 groundPos = new Vector3(ball.position.x, 18.5f, ball.position.z);
        transform.position = groundPos;
    }
}
