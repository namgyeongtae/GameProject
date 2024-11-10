using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIPlayer : UIScene
{
    Slider _healthSlider;
    Slider _damageSlider;
    Slider _xpSlider;

    Text _levelText;
    Text _healthText;
    Text _hpDescriptText;
    Text _xpText;
    Text _xpDescriptText;

    enum Sliders
    {
        HealthSlider,
        DamageSlider,
        XPSlider
    }

    enum Texts
    {
        LevelText,
        HealthText,
        HPDescriptText,
        XPText,
        XPDescriptText
    }

    public override void Init()
    {
        base.Init();

        Bind<Slider>(typeof(Sliders));
        Bind<Text>(typeof(Texts));

        _healthSlider = GetSlider((int)Sliders.HealthSlider);
        _damageSlider = GetSlider((int)Sliders.DamageSlider);
        _xpSlider = GetSlider((int)Sliders.XPSlider);

        _levelText = GetText((int)Texts.LevelText);
        _healthText = GetText((int)Texts.HealthText);
        _hpDescriptText = GetText((int)Texts.HPDescriptText);
        _xpText = GetText((int)Texts.XPText);
        _xpDescriptText = GetText((int)Texts.XPDescriptText);
    }

    public void TakeDamage(float damage)
    {
        _healthSlider.value -= damage;

        float targetValue = _healthSlider.value;

        _damageSlider.DOValue(targetValue, 0.2f).SetEase(Ease.Linear);

        _healthText.text = $"{_healthSlider.value}/{_healthSlider.maxValue}";
    }

    public void GetXP(int xp)
    {

    }

    public void UpdateHP(float currentHP, float maxHP)
    {
        _healthSlider.maxValue = maxHP;
        _damageSlider.maxValue = maxHP;

        _healthText.text = $"{currentHP}/{maxHP}";

    }

    public void UpdateLvl(int lvl)
    {
        _levelText.text = lvl.ToString();
    }

    public void UpdateXP(int currentXP, int maxXP)
    {
        _xpSlider.maxValue = maxXP;
        _xpSlider.value = currentXP;
        _xpText.text = $"{currentXP}/{maxXP}";
    }

    public void SetHPFull(float maxHP)
    {
        _healthSlider.maxValue = maxHP;
        _healthSlider.value = maxHP;
        _damageSlider.maxValue = maxHP;
        _damageSlider.value = maxHP;

        _healthText.text = $"{maxHP}/{maxHP}";
    }
}
