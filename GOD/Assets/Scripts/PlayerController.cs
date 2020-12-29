using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 10f;

    private Vector2 movementInput;
    private Rigidbody2D myRigidbody;
   
    private void Start() {
        movementInput = new Vector3( 0, 0, 0 );
        myRigidbody = GetComponent< Rigidbody2D >();
    }

    private void ReadInput() {
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

    private void Update() {
        ReadInput();
    }

    private void FixedUpdate() {
        Move(); 
    }

    private void OnTriggerEnter2D( Collider2D other ) {
        switch ( other.name ) {
            case "Point A":
                Debug.Log( "You're back on point A, go to point B");
                break;
            case "Point B":
                Debug.Log(  "Oh no, you're on point B. Hurry back to point A!" );
                break;
            default:
                Debug.Log( "Ignore " + other.name );
                break;
        }
    }

}