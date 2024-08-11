using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Manager
{
    public Camera MainCamera { get; private set; }

    public override void Init()
    {
        MainCamera = Camera.main;
    }
}
