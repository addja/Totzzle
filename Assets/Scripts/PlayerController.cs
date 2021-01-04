using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 10f;
    public float timeOut = 10f;
    public TimerManager timerManager;

    private Vector2 movementInput;
    private Rigidbody2D myRigidbody;
    private bool playerEnabled = true;
   
    private void Start() {
        movementInput = new Vector3( 0, 0, 0 );
        myRigidbody = GetComponent< Rigidbody2D >();
    }

    private void ReadInput() {
        if ( !playerEnabled ) {
            Debug.Log( "Player dead" );
            movementInput = new Vector2( 0, 0 );
            return;
        }
        // a w s d keys to move the player
        // alternatively arrow keys
        movementInput = new Vector2( Input.GetAxisRaw( "Horizontal" ), Input.GetAxisRaw( "Vertical" ) );
    }

    private void Move() {
        // move based on input
        Vector2 direction = movementInput.normalized;
        Vector2 velocity = direction * speed;

        // need to move using rigidbody to account for collisions with static objects
        myRigidbody.position += Time.fixedDeltaTime * velocity;;
    }

    private void CheckGameOver() {
        if ( timerManager.GameIsOver() ) {
            playerEnabled = false;
        }
    }

    private void Update() {
        if ( !playerEnabled ) {
            return;
        }
        CheckGameOver();
        ReadInput();
    }

    private void FixedUpdate() {
        Move(); 
    }

    private void StartTimer( float time ) {
        Debug.Log(  "Oh no, you're on point B. Hurry back to point A!" );
        timerManager.StartTimer( time );
    }

    private void StopTimer() {
        Debug.Log( "You're back on point A, go to point B" );
        if ( !timerManager.Started() ) {
            return; // We haven't started the timer yet
        }

        timerManager.StopTimer();
    }

    private void OnTriggerEnter2D( Collider2D other ) {
        switch ( other.name ) {
            case "Point A":
                StopTimer();
                break;
            case "Point B":
                StartTimer( timeOut );
                break;
            default:
                Debug.Log( "Ignore " + other.name );
                break;
        }
    }

}