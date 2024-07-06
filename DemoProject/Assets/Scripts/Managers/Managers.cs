using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers
{
    private static InputManager _input = new InputManager();

    public static InputManager Input;

    public void Init()
    {
        _input.Init();
    }

    public void Clear()
    {

    }
}
