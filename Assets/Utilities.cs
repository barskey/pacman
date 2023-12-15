using System;
using UnityEngine;

[Flags]
public enum TileExits
{
    None = 0,
    Right = 1,
    Down = 2,
    Left = 4,
    Up = 8
}
