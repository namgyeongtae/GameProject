using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : UIScene
{
    UIButton _testButton;

    enum Buttons
    {
        TestButton,
    }

    public override void Init()
    {
        base.Init();

        Bind<UIButton>(typeof(Buttons));

        _testButton = GetButton((int)Buttons.TestButton);
        _testButton.BindEvent(TestEvent);
    }

    public void TestEvent()
    {
        Debug.Log("aaaa");
    }
}
