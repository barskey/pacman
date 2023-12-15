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

    public TileController GetNextTile(Vector2 pos, Vector2 dir)
    {
        int row = (int)(pos.y + dir.y);
        int col = (int)(pos.x + dir.x);

        return tiles[row, col];
    }

    public TileController GetTileAt(Vector2 pos)
    {
        return tiles[(int)pos.y, (int)pos.x];
    }
}
