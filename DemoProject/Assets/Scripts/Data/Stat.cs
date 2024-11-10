using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats_", menuName = "Stat/Stats")]
public class Stat : ScriptableObject
{
    public int level;
    public float hp, attack, defense;
    public float attackRange;
    public float attackSpeed;
    public float detectRange;
    public float speed;
    public float knockBackForce;
    public float knockBackMultiplier = 1;
}
