using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject grid;

    private Dictionary<string, Tile> tileMap = new Dictionary<string, Tile>();

    private enum GameState
    {
        exploration, // pre queueLoaded
        queueLoaded, // ready to go to point b
        countdown // go to point a
    };
    private GameState gameState;

    private string TileIdentifier(int x, int y)
    {
        return x.ToString() + "/" + y.ToString();
    }

    private void Start()
    {
        gameState = GameState.exploration;

        foreach (Tile tile in grid.GetComponentsInChildren<Tile>())
        {
            Vector3 tilePosition = tile.transform.position;
            tileMap[TileIdentifier((int)tilePosition.x,(int)tilePosition.y)] = tile;
        }
    }

    public bool CanMove(int x, int y)
    {
        Tile tile;
        if (tileMap.TryGetValue(TileIdentifier(x, y), out tile) && tile != null)
        {
            switch (tile.type)
            {
                case Tile.TileType.origin:
                    // TODO: move this logic and other cases of this method
                    if (gameState == GameState.countdown)
                    {
                        Debug.Log("player wins");
                    }
                    break;
                case Tile.TileType.trigger:
                    if (gameState == GameState.queueLoaded)
                    {
                        Debug.Log("countdown begins");
                        gameState = GameState.countdown;
                        StartCountdown();
                    }
                    break;
                case Tile.TileType.setter:
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
        foreach (Tile tile in tileMap.Values)
        {
            if (tile != null)
            {
                tile.GetComponent<Tile>().UpdateTile();
            }
        }
        
    }

    public bool IsGameOver(int x, int y)
    {
        Tile landingTile = tileMap[TileIdentifier(x, y)];
        if (landingTile != null)
        {
            // TODO: Fix this in a more elegant way
            return landingTile.BadTile() && gameState == GameState.countdown;
        }

        return true;
    }

    void StartCountdown()
    {
        foreach (Tile tile in tileMap.Values)
        {
            if (tile != null)
            {
                tile.StartCountdown();
            }
        }
    }

}
