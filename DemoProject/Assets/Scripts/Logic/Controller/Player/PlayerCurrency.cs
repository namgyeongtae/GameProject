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
        // 예: UI 업데이트를 위한 이벤트 트리거
        Debug.Log("Currency updated! Gold: " + _gold + ", Diamonds: " + _diamonds);

        // 데이터 저장 메서드 호출 가능
        SaveCurrencyData();
    }

    private void SaveCurrencyData()
    {
        // 로컬 파일 저장 또는 서버 전송 로직
        Debug.Log("Currency data saved.");
    }

    private void LoadCurrencyData()
    {
        // 로컬 파일 또는 서버에서 데이터 로드
        Debug.Log("Currency data loaded.");

        // 여기서는 예시로 초기화
        _gold = 1000;
        _diamonds = 50;
    }
}
