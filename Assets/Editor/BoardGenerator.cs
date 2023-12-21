using System;
using UnityEngine;
using UnityEditor;

public struct TileInfo
{
    public TileInfo(bool hasDot, bool isPowerPellet, bool canMoveInto, int exits = 0)
    {
        HasDot = hasDot;
        IsPowerPellet = isPowerPellet;
        CanMoveInto = canMoveInto;
        Exits = exits;
    }

    public bool HasDot { get; }
    public bool IsPowerPellet { get; }
    public bool CanMoveInto { get; }
    public int Exits { get; set; }
}

public class MyTools
{
    [MenuItem("MyTools/GenerateTiles")]
    static void Create()
    {
        int rows = 36;
        int cols = 28;
        int tileSize = 8;

        UnityEngine.Object boardPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Board.prefab", typeof(GameObject));
        UnityEngine.Object tilePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Tile.prefab", typeof(GameObject));

        GameObject board = PrefabUtility.InstantiatePrefab(boardPrefab) as GameObject;

        GameObject tiles = new GameObject("Tiles");
        tiles.transform.parent = board.transform;

        var tileInfoArray = GetTileInfoArray(rows, cols);

        for (int row = 0; row < rows; row++)
        {
            GameObject rowGameobject = new GameObject($"Row_{row}");
            rowGameobject.transform.parent = tiles.transform;

            for (int col = 0; col < cols; col++)
            {
                string name = string.Format("Tile_{0}_{1}", row, col);

                GameObject tile = PrefabUtility.InstantiatePrefab(tilePrefab, rowGameobject.transform) as GameObject;
                tile.transform.position = new Vector2(col * tileSize, row * tileSize);
                tile.name = name;

                var t = tileInfoArray[row, col];

                var tc = tile.GetComponent<TileController>();
                tc.Setup(new Vector2(row, col), t.HasDot, t.IsPowerPellet, t.CanMoveInto, (TileExits)t.Exits);
            }
        }
    }

    static TileInfo[,] GetTileInfoArray(int rows, int cols)
    {
        TileInfo[,] tileArray = new TileInfo[rows, cols];

        string[] mazeString = {
            "                            ",
            "                            ",
            "                            ",
            "                            ",
            " ............  ............ ",
            " .    .     .  .     .    . ",
            " *    .     .  .     .    * ",
            " .    .     .  .     .    . ",
            " .......................... ",
            " .    .  .        .  .    . ",
            " .    .  .        .  .    . ",
            " ......  ....  ....  ...... ",
            "      .     +  +     .      ",
            "      .     +  +     .      ",
            "      .  ++++++++++  .      ",
            "      .  +        +  .      ",
            "      .  +        +  .      ",
            "++++++.+++        +++.++++++",
            "      .  +        +  .      ",
            "      .  +        +  .      ",
            "      .  ++++++++++  .      ",
            "      .  +        +  .      ",
            "      .  +        +  .      ",
            " ............  ............ ",
            " .    .     .  .     .    . ",
            " .    .     .  .     .    . ",
            " *..  .......++.......  ..* ",
            "   .  .  .        .  .  .   ",
            "   .  .  .        .  .  .   ",
            " ......  ....  ....  ...... ",
            " .          .  .          . ",
            " .          .  .          . ",
            " .......................... ",
            "                            ",
            "                            ",
            "                            "
        };

        int i = 0; // index to invert the map bottom-to-top so [0,0] is LL

        for (int r = rows - 1; r >= 0; r--)
        {
            for (int c = 0; c < cols; c++)
            {
                string x = mazeString[r][c].ToString();

                bool hasDot = false;
                bool isPowerPellet = false;
                bool canMoveInto = false;

                switch (x)
                {
                    case ".":
                        hasDot = true;
                        canMoveInto = true;
                        break;
                    case "*":
                        hasDot = true;
                        isPowerPellet = true;
                        canMoveInto = true;
                        break;
                    case "+":
                        canMoveInto = true;
                        break;
                }

                tileArray[i, c] = new TileInfo(hasDot, isPowerPellet, canMoveInto);
            }

            i++;
        }

        string[] exitString = {
            "                            ",
            "                            ",
            "                            ",
            "                            ",
            " 355557555556  355555755556 ",
            " A    A     A  A     A    A ",
            " A    A     A  A     A    A ",
            " A    A     A  A     A    A ",
            " 95555F55755D55D55755F5555E ",
            " A    A  A        A  A    A ",
            " A    A  A        A  A    A ",
            " 95555E  955755755C  B5555C ",
            "      A     A  A     A      ",
            "      A     A  A     A      ",
            "      A  355D55D556  A      ",
            "      A  A        A  A      ",
            "      A  A        A  A      ",
            "555555F55E        B55F555555",
            "      A  A        A  A      ",
            "      A  A        A  A      ",
            "      A  B55555555E  A      ",
            "      A  A        A  A      ",
            "      A  A        A  A      ",
            " 35555F55D556  355D55F55556 ",
            " A    A     A  A     A    A ",
            " A    A     A  A     A    A ",
            " 956  B55755D55955755E  35C ",
            "   A  A  A        A  A  A   ",
            "   A  A  A        A  A  A   ",
            " 35D55C  9556  355C  955D56 ",
            " A          A  A          A ",
            " A          A  A          A ",
            " 95555555555D55D5555555555C ",
            "                            ",
            "                            ",
            "                            "
        };

        i = 0; // index to invert the map bottom-to-top so [0,0] is LL

        for (int r = rows - 1; r >= 0; r--)
        {
            for (int c = 0; c < cols; c++)
            {
                string x = exitString[r][c].ToString();

                tileArray[i, c].Exits = (x != " ") ? Convert.ToInt32(x, 16) : 0;
            }

            i++;
        }

        return tileArray;
    }
}
