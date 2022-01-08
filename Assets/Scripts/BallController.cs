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
    Text showScore;
    GameObject endGamePanel;
    GameObject winView;
    GameObject loseView;

    void Start()
    {
        p1Score = 0;
        p2Score = 0;
        startJudge = false;
        lastWinner = 1;
        transform.position = new Vector3(0, 0, -200);
        canHit = true;

        showScore = GameObject.Find("Score").GetComponent<Text>();
        showScore.text = string.Format("{0:00}", p1Score) + " : " + string.Format("{0:00}", p2Score);

        resetTime = GameObject.Find("ResetTime").GetComponent<Text>();
        resetTime.enabled = false;

        endGamePanel = GameObject.Find("EndGame");

        winView = GameObject.Find("Win");
        loseView = GameObject.Find("Lose");

        endGamePanel.SetActive(false); ;
    }

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

        showScore.text = string.Format("{0:00}", p1Score) + " : " + string.Format("{0:00}", p2Score);
        if (p1Score == 15 || p2Score == 15)
        {
            Time.timeScale = 0;
            endGame();
        }
        else
        {
            //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Invoke("setPosition", 5);
            resetSecond = 6;
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

        showScore.text = string.Format("{0:00}", p1Score) + " : " + string.Format("{0:00}", p2Score);
        if (p1Score == 15 || p2Score == 15)
        {
            Time.timeScale = 0;
            endGame();
        }
        else
        {
            //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
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

        if(GameManager.curViewTeam == 1) {
            showScore.text = p1Score.ToString();

            if(lastWinner == 1) loseView.SetActive(false);
            else loseView.SetActive(false);
        }
        else {
            showScore.text = p2Score.ToString();

            if(lastWinner == 2) loseView.SetActive(false);
            else loseView.SetActive(false);
        }
    }
}
