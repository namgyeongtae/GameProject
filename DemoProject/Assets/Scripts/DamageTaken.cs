using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DamageTaken
{
    public float DamageAmount { get; private set; }
    public float KnockBackForce { get; private set; }
    public GameObject Source { get; private set; }

    public DamageTaken(float damageAmount, float knockBackForce, GameObject source)
    {
        DamageAmount = damageAmount;
        KnockBackForce = knockBackForce;
        Source = source;
    }
}
