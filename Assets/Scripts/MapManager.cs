using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public GameObject tilePrefab;
    public char[,] tileMapBluePrint = {
        { '1','c','1' },
        { 'a','2','b' },
        { '1','1','1' },
    };

    GameObject[,] tileMap;
    uint mapSizeX;
    uint mapSizeY;
    enum GameState
    {
        exploration, // go to point c
        queueLoaded, // go to point b
        countdown // go to point a
    };
    private GameState gameState;

    void Start()
    {
        gameState = GameState.exploration;
        mapSizeX = (uint)tileMapBluePrint.GetLength(0);
        mapSizeY = (uint)tileMapBluePrint.GetLength(1);
        tileMap = new GameObject[mapSizeX, mapSizeY];

        for (uint x = 0; x < mapSizeX; x++)
        {
            for (uint y = 0; y < mapSizeY; y++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                newTile.GetComponent< Tile >().SetTile(tileMapBluePrint[x, y]);
                tileMap[x, y] = newTile;
            }
        }
    }

    public bool CanMove(uint x, uint y)
    {
        if (x < mapSizeX && y < mapSizeY && tileMap.GetValue(x, y) != null)
        {
            switch (tileMapBluePrint.GetValue(x, y))
            {
                case 'a':
                    if (gameState == GameState.countdown)
                    {
                        Debug.Log("player wins");
                    }
                    break;
                case 'b':
                    if (gameState == GameState.queueLoaded)
                    {
                        Debug.Log("countdown begins");
                        gameState = GameState.countdown;
                        StartCountdown();
                    }
                    break;
                case 'c':
                    if (gameState == GameState.exploration)
                    {
                        gameState = GameState.queueLoaded;
                        Debug.Log("queue loaded");
                    }
                    break;
                default:
                    // nothing
                    break;
            }

            return true;
        }

        return false;
    }

    public void UpdateWorld()
    {
        foreach (GameObject tile in tileMap)
        {
            if (tile != null)
            {
                tile.GetComponent<Tile>().UpdateTile();
            }
        }
        
    }

    public bool IsGameOver(uint x, uint y)
    {
        GameObject landingTile = tileMap[x, y];
        if (landingTile != null)
        {
            // TODO: Fix this in a more elegant way
            return landingTile.GetComponent<Tile>().BadTile() && gameState == GameState.countdown;
        }

        return true;
    }

    void StartCountdown()
    {
        foreach (GameObject tile in tileMap)
        {
            tile.GetComponent<Tile>().StartCountdown();
        }
    }

}
