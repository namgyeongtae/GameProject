using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Utils
{
    public static float AngleBetweenTwoPoints(Vector2 from, Vector2 to)
    {
        Vector2 direction = to - from;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
