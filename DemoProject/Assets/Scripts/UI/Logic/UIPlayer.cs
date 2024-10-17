using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : UIScene
{
    Slider _healthSlider;
    Slider _damageSlider;
    Slider _xpSlider;

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

        _healthText = GetText((int)Texts.HealthText);
        _hpDescriptText = GetText((int)Texts.HPDescriptText);
        _xpText = GetText((int)Texts.XPText);
        _xpDescriptText = GetText((int)Texts.XPDescriptText);
    }
}
