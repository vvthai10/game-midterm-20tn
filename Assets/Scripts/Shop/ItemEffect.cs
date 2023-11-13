using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    public static ItemEffect instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void OnItemBuy(int itemIdx)
    {
        switch(itemIdx)
        {
            case 0:
                Potion();
                break;
            case 1:
                StaminaSpeed();
                break;
            case 2:
                HealthLength();
                break;
            case 3:
                StaminaLength();
                break;
        }
    }

    private void Potion()
    {
        main_character.instance.IncreaseFlask(1);
    }

    private void StaminaSpeed()
    {
        StaminaBar.instance.IncreseStaminaRegenSpeed(0.05f);
    }

    private void HealthLength()
    {
        HealthBar.instance.IncreaseMaxHealth(15f);
    }

    private void StaminaLength()
    {
        StaminaBar.instance.IncreaseStaminaAmount(25f);
    }
}
