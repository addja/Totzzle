using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public GameObject tilePrefab;
    public int[,] tileMapBluePrint = {
        { 1, 1, 1 },
        { 1, 2, 1 },
        { 1, 1, 1 },
    };

    GameObject[,] tileMap;
    uint mapSizeX;
    uint mapSizeY;

    void Start()
    {
        mapSizeX = (uint)tileMapBluePrint.GetLength(0);
        mapSizeY = (uint)tileMapBluePrint.GetLength(1);
        tileMap = new GameObject[mapSizeX, mapSizeY];

        for (uint x = 0; x < mapSizeX; x++)
        {
            for (uint y = 0; y < mapSizeY; y++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                // newTile.property = tileMapBluePrint[i, j];
                tileMap[x, y] = newTile;
            }
        }
    }

    public bool CanMove(uint x, uint y)
    {
        return x < mapSizeX && y < mapSizeY && tileMap.GetValue(x, y) != null;
    }

}
