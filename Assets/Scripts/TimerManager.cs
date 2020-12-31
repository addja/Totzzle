using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour {

    public GameObject timerCanvas;
    public Text timerText;
    public Text countdownText;

    private bool started = false;
    private float timeLeft = 0f;

    public void StopTimer() {
        Debug.Log( "Stopped timer" );
        timerCanvas.SetActive( false );
        started = false;
    }

    public void StartTimer( float time ) {
        if ( started ) {
            return;
        }

        timeLeft = time;
        Debug.Log( "Started timer with timeout: " + timeLeft );
        started = true;
        timerText.text = "Oh no! Go back to red!";
        countdownText.text = Mathf.CeilToInt( timeLeft ).ToString();
        timerCanvas.SetActive( true );
    }

    private void Update() {
        if ( !started ) {
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
        started = false;
        timerText.text = "you lost dude";
    }
}
