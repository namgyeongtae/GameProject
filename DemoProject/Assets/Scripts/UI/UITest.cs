using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class UITest : UIScene
{
    UIButton _testButton;

    Image _testImage;

    Vector2 _originPos;

    enum Buttons
    {
        TestButton,
    }

    enum Images
    {
        TestImage
    }

    public override void Init()
    {
        base.Init();

        Bind<UIButton>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        _testButton = GetButton((int)Buttons.TestButton);
        _testButton.BindEvent(TestEvent);

        _testImage = GetImage((int) Images.TestImage);

        gameObject.BindEvent(OnBeginDrag, Define.UIEvent.BeginDrag);
        gameObject.BindEvent(OnDrag, Define.UIEvent.Drag);
        gameObject.BindEvent(OnEndDrag, Define.UIEvent.EndDrag);
    }

    public void TestEvent()
    {
        Debug.Log("aaaa");
    }

    private void OnBeginDrag(PointerEventData eventData)
    {
        _originPos = _testImage.rectTransform.position;
    }

    private void OnDrag(PointerEventData eventData)
    {
        _testImage.rectTransform.position = eventData.position;
    }

    private void OnEndDrag(PointerEventData eventData)
    {
        _testImage.rectTransform.position = _originPos;
    }
}
