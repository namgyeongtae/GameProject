using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIInventory : UIPopup
{
    private List<UISlot> _itemSlots = new List<UISlot>();

    private Text _silverCoin;
    private Text _goldCoin;
    private Text _diamond;

    public UISlot GetSlot(int index) => _itemSlots[index];

    enum Texts
    {
        SilverCoin,
        GoldCoin,
        Diamond
    }

    public override void Init()
    {
        base.Init();

        BindUI();

        DisplayPlayerItem();

        transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(400f, 0f, 0f);
    }

    protected override void BindUI()
    {
        Bind<Text>(typeof(Texts));

        _silverCoin = GetText((int)Texts.SilverCoin);
        _goldCoin = GetText((int)Texts.GoldCoin);
        _diamond = GetText((int)Texts.Diamond);

        _itemSlots = GetComponentsInChildren<UISlot>().ToList();
    }

    private void DisplayPlayerItem()
    {
        _silverCoin.text = PlayerCurrency.Instance.Silver.ToString();
        _goldCoin.text = PlayerCurrency.Instance.Gold.ToString();
        _diamond.text = PlayerCurrency.Instance.Diamonds.ToString();
    }

    private void UpdateItem(UISlot item)
    {

    }
}
