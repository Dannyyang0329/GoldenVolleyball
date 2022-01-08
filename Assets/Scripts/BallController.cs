using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    // judge variables
    int courtNum;
    public bool beenHit = false;
    public bool startJudge;

    // score variables
    int p1Score;
    int p2Score;
    Text showScore;

    // reset variables
    int lastWinner;
    float resetSecond;
    public bool canHit;
    Text resetTime;

    // end game
    GameObject endGamePanel;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, -200);

        p1Score = 0;
        p2Score = 0;
        
        displayScore();

        canHit = true;
        startJudge = false;

        resetTime = GameObject.Find("ResetTime").GetComponent<Text>();
        resetTime.enabled = false;

        endGamePanel = GameObject.Find("EndGame");
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
        // invisible wall
        if (collision.gameObject.CompareTag("OutOfCourt") && startJudge)
        {
            startJudge = false;
            judgeOutWinner();
        }
        // int the court
        else if (collision.gameObject.CompareTag("Ground") && startJudge)
        {
            startJudge = false;
            judgeInWinner();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // out of court
        if (other.gameObject.CompareTag("OutOfCourt") && startJudge)
        {
            startJudge = false;
            judgeOutWinner();
        }
    }

    // out of court
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
        displayScore();
        checkEndGame();
    }

    // in the court
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
        displayScore();
        checkEndGame();
    }

    private void displayScore() {
        showScore = GameObject.Find("Score").GetComponent<Text>();
        showScore.text = string.Format("{0:00}", p1Score) + " : " + string.Format("{0:00}", p2Score);
    }

    private void checkEndGame()
    {
        if (p1Score == 10 || p2Score == 10)
        {
            Time.timeScale = 0;
            endGame();
        }
        else
        {
            Invoke("setPosition", 5);
            resetSecond = 6;
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
