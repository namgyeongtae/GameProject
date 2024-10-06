using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public static float KNOCKBACK_FORCE = 5.0f;
    public static float INVINCIBLE_TIME = 1.0f;

    public static int AFTER_IMAGE_POOL_COUNT = 15;

    public static int DROP_MIN = 3;
    public static int DROP_MAX = 6;

    public enum WeaponType
    {
        Sword,
        Bow,
        Wand
    }

    public enum CollectableType
    {
        CoinSilver,
        CoinGold,
        Diamond
    }

    public enum OrderLayer
    {
        NONE = 0,
        FLOOR = 1,
        WALL = 2,
        DECO = 3,
        ENTITY = 4
    }


    public enum UIEvent
    {
        Click,
        Pressed,
        PointerUp,
        PointerDown,
        Drag,
        BeginDrag,
        EndDrag
    }

    public enum MouseEvent
    {
        Press,
        Click,
    }
}
