using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject grid;

    private Dictionary<string, Tile> tileMap = new Dictionary<string, Tile>();

    private Vector2Int playerPosition = new Vector2Int();
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
            tileMap[TileIdentifier((int)tilePosition.x, (int)tilePosition.y)] = tile;
        }
    }

    public void QueueLoaded()
    {
        if (gameState == GameState.exploration)
        {
            // assumption: Player is on a valid tile
            Tile tile;
            tileMap.TryGetValue(TileIdentifier(playerPosition.x, playerPosition.y), out tile);
            if (tile.type == Tile.TileType.trigger)
            {
                StartCountdown();
            }
            else
            {
                gameState = GameState.queueLoaded;
            }
        }
    }

    public void NewPlayerPosition(int x, int y)
    {
        playerPosition = new Vector2Int(x, y);
        Tile tile; // assumption: Player moves to a valid tile
        tileMap.TryGetValue(TileIdentifier(x, y), out tile);
        switch (tile.type)
        {
            case Tile.TileType.origin:
                if (gameState == GameState.countdown)
                {
                    Debug.Log("player wins");
                }
                break;
            case Tile.TileType.trigger:
                if (gameState == GameState.queueLoaded)
                {
                    StartCountdown();
                }
                break;
            default:
                // nothing
                break;
        }
    }

    public bool CanMove(int x, int y)
    {
        Tile tile; // assumption: Player moves to a valid tile
        return (tileMap.TryGetValue(TileIdentifier(x, y), out tile) && tile != null);
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

    void StartCountdown()
    {
        gameState = GameState.countdown;
        foreach (Tile tile in tileMap.Values)
        {
            if (tile != null)
            {
                tile.StartCountdown();
            }
        }
    }

}
