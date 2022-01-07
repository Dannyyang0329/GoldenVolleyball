using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    int courtNum;
    public bool beenHit = false;
    public bool startJudge;
    public bool canHit;
    int p1Score;
    int p2Score;
    int lastWinner;
    float resetSecond;
    Text resetTime;
    GameObject endGamePanel;

    // Start is called before the first frame update
    void Start()
    {
        p1Score = 0;
        p2Score = 0;
        startJudge = false;
        lastWinner = 1;
        transform.position = new Vector3(0, 0, -200);
        canHit = true;
        resetTime = GameObject.Find("ResetTime").GetComponent<Text>();
        endGamePanel = GameObject.Find("EndGame");
        resetTime.enabled = false;
        endGamePanel.SetActive(false); ;
    }

    // Update is called once per frame
    void Update()
    {
        if (resetTime.enabled)
        {
            resetSecond -= Time.deltaTime;
            resetTime.text = "Reset Time : " + string.Format("{0:00}", 0) + " : " + string.Format("{0:00}", (int)resetSecond);
            if (resetSecond < 1) resetTime.enabled = false;
        }

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

        if (p1Score == 25 || p2Score == 25)
        {
            Time.timeScale = 0;
            endGame();
        }
        else
        {
            //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Invoke("setPosition", 10);
            resetSecond = 11;
            resetTime.text = "Reset Time : " + string.Format("{0:00}", 0) + " : " + string.Format("{0:00}", (int)resetSecond);
            resetTime.enabled = true;
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

        if (p1Score == 25 || p2Score == 25)
        {
            Time.timeScale = 0;
            endGame();
        }
        else
        {
            //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Invoke("setPosition", 10);
            resetSecond = 11;
            resetTime.text = "Reset Time : " + string.Format("{0:00}", 0) + " : " + string.Format("{0:00}", (int)resetSecond);
            resetTime.enabled = true;
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
        Debug.Log("reset");
    }

    public void setStart()
    {
        Invoke("openJudge", Time.deltaTime * 10);
    }

    private void openJudge()
    {
        startJudge = true;
    }

    private void endGame()
    {
        endGamePanel.SetActive(true);
    }
}
