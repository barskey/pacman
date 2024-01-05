using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public TileController[,] tiles;

    private void Start()
    {
        Setup();
    }

    private void Setup() {
        tiles = new TileController[36, 28];

        foreach (var tc in GetComponentsInChildren<TileController>())
        {
            tiles[(int)tc.coord.x, (int)tc.coord.y] = tc;
        }
    }

    public TileController GetTileAtCoord(Vector2 coord)
    {
        return tiles[(int)coord.x, (int)coord.y];
    }

    public TileController GetTileAtPos(Vector2 pos)
    {
        // center coord of tile is divisible by 8
        // need to offset pos by 3 to acount for center pixel at 3,3
        // x is col, y is row
        Vector2 coord = new Vector2(((int)pos.y + 3) / 8, ((int)pos.x + 3) / 8);

        return GetTileAtCoord(coord);
    }

    public TileController GetTileAtExit(Vector2 coord, Vector2 exit)
    {
        int row = (int)(coord.x + exit.y);
        int col = (int)(coord.y + exit.x);

        return tiles[row, col];
    }
}
