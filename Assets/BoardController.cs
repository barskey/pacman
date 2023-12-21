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

    public TileController GetTileAtExit(Vector2 coord, Vector2 exit)
    {
        int row = (int)(coord.x + exit.y);
        int col = (int)(coord.y + exit.x);

        return tiles[row, col];
    }

    public TileController GetTileAt(Vector2 coord)
    {
        return tiles[(int)coord.x, (int)coord.y];
    }
}
