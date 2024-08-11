using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats_", menuName = "Stat/StatData")]
public class Stat : ScriptableObject
{
    public float hp, attack, defense;
    public float speed;
    public float detectRange;
}