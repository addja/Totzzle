using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class TimerManager : MonoBehaviour {

    public GameObject timerCanvas;
    public Text timerText;
    public Text countdownText;
    public GameObject replayText;

    private bool countdownStarted = false;
    private float timeLeft = 0f;
    private bool gameOver = false;

    public bool Started() {
        return countdownStarted;
    }

    public void StopTimer() {
        Debug.Log( "Stopped timer" );
        gameOver = true;
        timerText.text = "Congrats! You made it!";
        countdownText.text = "";
        replayText.SetActive( true );
    }

    public void StartTimer( float time ) {
        if ( countdownStarted ) {
            return;
        }

        timeLeft = time;
        Debug.Log( "Started timer with timeout: " + timeLeft );
        countdownStarted = true;
        timerText.text = "Oh no! Go back to red!";
        countdownText.text = Mathf.CeilToInt( timeLeft ).ToString();
        timerCanvas.SetActive( true );
    }

    public bool GameIsOver() {
        return gameOver;
    }

    private void Update() {
        if ( !countdownStarted ) {
            return;
        }
        
        if ( gameOver ) {
            if ( Input.GetKeyDown( KeyCode.R ) ) {
                // restart level
                SceneManager.LoadScene( SceneManager.GetActiveScene().name );
            }
            return;
        }

        timeLeft -= Time.deltaTime;
        countdownText.text = Mathf.CeilToInt( timeLeft ).ToString();

        if ( timeLeft <= 0 ) {
            GameOver();
        }
    }

    private void GameOver() {
        Debug.Log( "Game over" );
        gameOver = true;
        timerText.text = "you lost dude";
        replayText.SetActive( true );
    }
} 