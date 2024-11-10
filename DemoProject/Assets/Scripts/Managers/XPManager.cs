using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : Manager
{
    private PlayerStat _playerStat = new PlayerStat();

    public override void Init()
    {
    }

    public void AddXP(int amount)
    {
        Managers.Character.UserData.Exp += amount;

        int currentXP = Managers.Character.UserData.Exp;
        int requiredXP = Managers.Data.XP.XP_Dict[Managers.Character.UserData.Level].exp_required;

        if (currentXP >= requiredXP)
        {
            currentXP -= requiredXP;
            Managers.Character.UserData.Exp = currentXP;
            LevelUP();
        }

        _playerStat.SaveUserData();

        var playerUI = Managers.UI.GetUI<UIPlayer>();
        playerUI.UpdateXP(currentXP, requiredXP);
    }

    public void LevelUP()
    {
        int currentLevel = Managers.Character.UserData.Level;

        var data = Managers.Data.XP.XP_Dict[currentLevel];

        Managers.Character.UserData.Character.MaxHP = data.hp;
        Managers.Character.UserData.Character.HP = data.hp;
        Managers.Character.UserData.Character.Attack = data.attack;
        Managers.Character.UserData.Character.Defense = data.defense;
        Managers.Character.UserData.Character.Speed = data.speed;
        Managers.Character.UserData.Level = currentLevel + 1;

        _playerStat.SaveUserData();
        
        // Update Player Info UI
        var playerUI = Managers.UI.GetUI<UIPlayer>();
        playerUI.SetHPFull(data.hp);
        playerUI.UpdateLvl(currentLevel);

        Managers.Character.Player.CurrentHP = data.hp;
    }

    public void DisplayUpStats()
    {
        
    }
}
