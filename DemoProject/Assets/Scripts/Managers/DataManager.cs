using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Manager
{
    private XPTable _xp = new();


    public XPTable XP => _xp;

    public override void Init()
    {
        _xp.Init();
    }
}
