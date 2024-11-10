using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrency
{
    private int _silver;
    private int _gold;
    private int _diamonds;

    public int Silver => _silver;
    public int Gold => _gold;
    public int Diamonds => _diamonds;

    private static PlayerCurrency _instance;

    public static PlayerCurrency Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerCurrency();
            }
            return _instance;
        }
    }

    private PlayerCurrency()
    {
        LoadCurrencyData();
    }

    public void AddSilver(int amount)
    {
        _silver += amount;
        OnCurrencyChanged();
    }

    public bool SpendSilver(int amount)
    {
        if (_silver >= amount)
        {
            _silver -= amount;
            OnCurrencyChanged();
            return true;
        }

        return false;
    }

    public void AddGold(int amount)
    {
        _gold += amount;
        OnCurrencyChanged();
    }

    public bool SpendGold(int amount)
    {
        if (_gold >= amount)
        {
            _gold -= amount;
            OnCurrencyChanged();
            return true;
        }

        return false;
    }

    public void AddDiamonds(int amount)
    {
        _diamonds += amount;
        OnCurrencyChanged();
    }

    public bool SpendDiamonds(int amount)
    {
        if (_diamonds >= amount)
        {
            _diamonds -= amount;
            OnCurrencyChanged();
            return true;
        }

        return false;
    }

    private void OnCurrencyChanged()
    {
        // ��: UI ������Ʈ�� ���� �̺�Ʈ Ʈ����
        Debug.Log("Currency updated! Gold: " + _gold + ", Diamonds: " + _diamonds);

        // ������ ���� �޼��� ȣ�� ����
        SaveCurrencyData();
    }

    private void SaveCurrencyData()
    {
        // ���� ���� ���� �Ǵ� ���� ���� ����
        Debug.Log("Currency data saved.");
    }

    private void LoadCurrencyData()
    {
        // ���� ���� �Ǵ� �������� ������ �ε�
        Debug.Log("Currency data loaded.");

        // ���⼭�� ���÷� �ʱ�ȭ
        _gold = 1000;
        _diamonds = 50;
    }
}
