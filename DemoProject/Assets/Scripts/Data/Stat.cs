using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats_", menuName = "Stat/Stats")]
public class Stat : ScriptableObject
{
    public float hp, attack, defense;
    public float speed;
    public float detectRange;
}
