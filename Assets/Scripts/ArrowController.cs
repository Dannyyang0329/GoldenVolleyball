using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("volleyball");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(ball.transform.position.x, 18.5f, ball.transform.position.z);
    }
}
