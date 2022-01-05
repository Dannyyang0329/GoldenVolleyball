using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    int courtNum;
    public bool beenHit = false;
    public bool startJudge;
    int p1Score;
    int p2Score;
    int lastWinner;
    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        p1Score = 0;
        p2Score = 0;
        characterController = GetComponent<CharacterController>();
        startJudge = false;
        lastWinner = 1;
        transform.position = new Vector3(0, 15, -200);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.position.z < 0)
        {
            if (courtNum == 2) beenHit = false;
            courtNum = 1;
        }
        else
        {
            if (courtNum == 1) beenHit = false;
            courtNum = 2;
        }

        if (characterController.isGrounded && startJudge)
        {
            startJudge = false;
            judgeWinner();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("OutOfCourt")&&startJudge)
        {
            startJudge = false;
            judgeWinner();
        }
        else if (collision.gameObject.CompareTag("Ground") && startJudge)
        {
            startJudge = false;
            judgeWinner();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OutOfCourt")&&startJudge)
        {
            startJudge = false;
            judgeWinner();
        }
    }

    private void judgeWinner()
    {
        if (courtNum == 1 && beenHit) // 2 win
        {
            p2Score++;
            lastWinner = 2;
        }
        else if (courtNum == 1 && !beenHit) // 1 win
        {
            p1Score++;
            lastWinner = 1;
        }
        else if (courtNum == 2 && beenHit) // 1 win
        {
            p1Score++;
            lastWinner = 1;
        }
        else if (courtNum == 2 && !beenHit) // 2 win
        {
            p2Score++;
            lastWinner = 2;
        }

        if (p1Score == 5 || p2Score == 5)
        {
            //endGame();
        }
        else
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            if (lastWinner == 1)
            {
                transform.position = new Vector3(0, 0, -200);

            }
            else
            {
                transform.position = new Vector3(0, 0, 200);
            }
        }
    }
}
