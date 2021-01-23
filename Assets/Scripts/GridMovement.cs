using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{

    public float timeToMove = .2f;
    public MapManager mapManager;

    bool isMoving;
    bool gameOver = false;
    Vector2 origPosition, targetPosition;

    void Update()
    {
        if (!isMoving && !gameOver)
        {
            processInput();
        }
    }

    void processInput()
    {
        Vector2 direction = Vector2.zero;

        // If deployed, should use GetAxisRaw for plaform neutrality
        if (Input.GetKey(KeyCode.W))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = Vector2.right;
        }
        else
        {
            return;
        }

        StartCoroutine(MovePlayer(direction));
    }

    // fancy coroutine
    IEnumerator MovePlayer(Vector2 direction)
    {
        origPosition = transform.position;
        targetPosition = origPosition + direction;

        if (mapManager.CanMove((uint)targetPosition.x, (uint)targetPosition.y))
        {

            isMoving = true;
            float ellapsedTime = 0;
            while (ellapsedTime < timeToMove)
            {
                transform.position = Vector2.Lerp(origPosition, targetPosition, (ellapsedTime / timeToMove));
                ellapsedTime += Time.deltaTime;
                yield return null;
            }

            // make sure there is no small jitter from lerp on final position
            transform.position = targetPosition;

            isMoving = false;
            mapManager.UpdateWorld();
            gameOver = mapManager.IsGameOver((uint)targetPosition.x, (uint)targetPosition.y);
        }
    }
}
