using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;

public class BallController : NetworkBehaviour
{
    // judge variables
    int courtNum = 1;
    public bool beenHit = false;
    public bool startJudge = false;

    // score variables
    int p1Score = 0;
    int p2Score = 0;
    Text showScore;

    // reset variables
    int lastWinner = 1;
    float resetSecond = 0;
    public bool canHit = true;
    Text resetTime;

    // end game
    GameObject endGamePanel;
    GameObject winView;
    GameObject loseView;

    void Start()
    {
        transform.position = new Vector3(0, 0, -200);

        canHit = true;
        displayScore(p1Score, p2Score);
        displayCountdown(resetSecond);


        endGamePanel = GameObject.Find("EndGame");
        winView = GameObject.Find("Win");
        loseView = GameObject.Find("Lose");

        endGamePanel.SetActive(false);
    }

    void Update()
    {
        if (!resetTime.text.Equals(" "))
        {
            resetSecond -= Time.deltaTime;

            displayCountdown(resetSecond);
            ResetTimeClientRpc(resetSecond);

            if (resetSecond < 1) resetTime.text = " ";
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
        // in the court
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

        displayScore(p1Score, p2Score);
        DisplayScoreClientRpc(p1Score, p2Score);

        checkEndGame(lastWinner, p1Score, p2Score);
        CheckEndGameClientRpc(lastWinner, p1Score, p2Score);
    }

    // in the court
    private void judgeInWinner()
    {
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

        displayScore(p1Score, p2Score);
        DisplayScoreClientRpc(p1Score, p2Score);

        checkEndGame(lastWinner, p1Score, p2Score);
        CheckEndGameClientRpc(lastWinner, p1Score, p2Score);
    }

    private void displayScore(int p1Score, int p2Score) {
        if(GameManager.curViewTeam == 1) {
            if(!showScore) showScore = GameObject.Find("Score").GetComponent<Text>();
            showScore.text = string.Format("{0:00}", p1Score) + " : " + string.Format("{0:00}", p2Score);
        }
        else {
            if(!showScore) showScore = GameObject.Find("Score").GetComponent<Text>();
            showScore.text = string.Format("{0:00}", p2Score) + " : " + string.Format("{0:00}", p1Score);
        }
    }

    private void checkEndGame(int lastWinner, int p1Score, int p2Score)
    {
        if (p1Score == 10 || p2Score == 10)
        {
            Time.timeScale = 0;

            int score = (GameManager.curViewTeam == 1) ? p1Score : p2Score;
            endGame(lastWinner);
            GameObject.Find("ShowScore").GetComponent<Text>().text = score.ToString();
        }
        else
        {
            Invoke("setPosition", 5);

            resetSecond = 6;
            // SetResetSecondClientRpc();

            displayCountdown(resetSecond);
            ResetTimeClientRpc(resetSecond);
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
    }

    public void setStart()
    {
        startJudge = true;
    }

    private void endGame(int lastWinner)
    {
        endGamePanel.SetActive(true);

        if(GameManager.curViewTeam == 1) {
            showScore.text = p1Score.ToString();

            if(lastWinner == 1) loseView.SetActive(false);
            else winView.SetActive(false);
        }
        else {
            showScore.text = p2Score.ToString();

            if(lastWinner == 2) loseView.SetActive(false);
            else winView.SetActive(false);
        }
    }

    private void displayCountdown(float resetSecond) {
        if(!resetTime) resetTime = GameObject.Find("ResetTime").GetComponent<Text>();
        resetTime.text = "Reset Time : " + string.Format("{0:00}", 0) + " : " + string.Format("{0:00}", (int)resetSecond);
    }

    [ClientRpc]
    void ResetTimeClientRpc(float resetSec) {
        if (IsServer) return;

        displayCountdown(resetSec);
    }

    [ClientRpc]
    void DisplayScoreClientRpc(int p1Score, int p2Score) {
        displayScore(p1Score, p2Score);
    }

    [ClientRpc]
    void CheckEndGameClientRpc(int lastWinner, int p1S, int p2S) {
        if (IsServer) return;

        checkEndGame(lastWinner, p1S, p2S);
    }
}
