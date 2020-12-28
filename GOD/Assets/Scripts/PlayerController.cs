using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 10f;

    private Vector3 movementInput = new Vector3( 0, 0, 0 );
   
    private void ReadInput() {
        // a w s d keys to move the player
        // alternatively arrow keys
        movementInput = new Vector3( Input.GetAxisRaw( "Horizontal" ), Input.GetAxisRaw( "Vertical" ), 0 );
    }

    private void Move() {
        // move based on input
        Vector3 direction = movementInput.normalized;
        Vector3 velocity = direction * speed;
        transform.position += Time.fixedDeltaTime * velocity;;
    }

    private void Update() {
        ReadInput();
    }

    private void FixedUpdate() {
        Move(); 
    }

}