using System;
using UnityEngine;

public static class Constants
{
    public const float MaxSpeedInPPS = 75.75757625f;
}


[Flags]
public enum TileExits
{
    None = 0,
    Right = 1,
    Down = 2,
    Left = 4,
    Up = 8
}
