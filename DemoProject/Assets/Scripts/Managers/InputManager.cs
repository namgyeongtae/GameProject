using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Manager
{
    public override void Init()
    {
        
    }

    public override void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            var inventory = Managers.UI.GetUI<UIInventory>();
            if (inventory == null ) 
                Managers.UI.ShowPopupUI<UIInventory>();
            else
                Managers.UI.ClosePopupUI(inventory);
        }
    }

    public override void Clear()
    {
        
    }
}
