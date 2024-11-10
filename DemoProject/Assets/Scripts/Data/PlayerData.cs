using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    public int Level;
    public int Exp;
    public CurrencyData Currency;
    public CharacterData Character;
    // public InventoryData Inventory;
}

[Serializable]
public class CurrencyData
{
    public int silver;
    public int gold;
    public int diamond;
}

[Serializable]
public class InventoryData
{
    // 장비
    // 소비
    // .... 
}

[Serializable]
public class CharacterData
{
    public float HP;
    public float MaxHP;
    public float Attack;
    public float Defense;
    public float KnockbackForce;
    public float Speed;
}