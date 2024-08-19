using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public static float KNOCKBACK_FORCE = 5.0f;
    public static float INVINCIBLE_TIME = 1.0f;

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
