using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    int courtNum;
    public bool beenHit = false;
    public bool startJudge;
    public bool canHit;
    int p1Score;
    int p2Score;
    int lastWinner;

    // Start is called before the first frame update
    void Start()
    {
        p1Score = 0;
        p2Score = 0;
        startJudge = false;
        lastWinner = 1;
        transform.position = new Vector3(0, 0, -200);
        canHit = true;
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

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("OutOfCourt")&&startJudge)
        {
            startJudge = false;
            judgeOutWinner();
        }
        else if (collision.gameObject.CompareTag("Ground") && startJudge)
        {
            startJudge = false;
            judgeInWinner();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OutOfCourt")&&startJudge)
        {
            startJudge = false;
            judgeOutWinner();
        }
    }

    private void judgeOutWinner()
    {
        Debug.Log("win");
        canHit = false;
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
            //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Invoke("setPosition", 10);
            
        }
    }

    private void judgeInWinner()
    {
        Debug.Log("win");
        canHit = false;
        if (courtNum == 1) // 2 win
        {
            p2Score++;
            lastWinner = 2;
        }
        else if (courtNum == 2) // 1 win
        {
            p1Score++;
            lastWinner = 1;
        }

        if (p1Score == 5 || p2Score == 5)
        {
            //endGame();
        }
        else
        {
            //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Invoke("setPosition", 10);

        }
    }

    private void setPosition()
    {
        if (lastWinner == 1)
        {
            transform.position = new Vector3(0, 100, -200);

        }
        else
        {
            transform.position = new Vector3(0, 100, 200);
        }
        canHit = true;
        Debug.Log("----");
    }

    public void setStart()
    {
        Invoke("openJudge", Time.deltaTime * 10);
    }

    private void openJudge()
    {
        startJudge = true;
    }
}
