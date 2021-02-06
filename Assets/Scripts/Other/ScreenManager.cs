using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public Transform gameScreen;
    public Transform hudScreen;

    private Vector3 screenResizeScale = new Vector3(-2, 0, 0);
    private Vector3 screenResizePosition = new Vector3(-1, 0, 0);

    private bool focusGame;

    private void Start() {
        focusGame = true;
    }

    private void UpdateScreens()
    {
        if (focusGame)
        {
            // make queue small
            gameScreen.localScale -= screenResizeScale;
            gameScreen.position -= screenResizePosition;
            hudScreen.localScale += screenResizeScale;
            hudScreen.position -= screenResizePosition;
        }
        else
        {
            // make queue big
            gameScreen.localScale += screenResizeScale;
            gameScreen.position += screenResizePosition;
            hudScreen.localScale -= screenResizeScale;
            hudScreen.position += screenResizePosition;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            focusGame = !focusGame;
            UpdateScreens();
        }
    }
}
