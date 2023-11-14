using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData
{
    public const string MAX_HEALTH_KEY = "max health";
    public const string MAX_STAMINA_KEY = "max stamina";
    public const string STAMINA_REGEN_SPEED_KEY = "stamina regen";
    public const string FLASK_AMOUNT_KEY = "flask amount";

    public static void saveMaxHealth(float value)
    {
        PlayerPrefs.SetFloat(MAX_HEALTH_KEY, value);
    }

    public static void saveMaxStamina(float value)
    {
        PlayerPrefs.SetFloat(MAX_STAMINA_KEY, value);
    }

    public static void saveStaminaRegenSpeed(float value)
    {
        PlayerPrefs.SetFloat(STAMINA_REGEN_SPEED_KEY, value);
    }

    public static void saveFlaskAmount(int value)
    {
        PlayerPrefs.SetInt(FLASK_AMOUNT_KEY, value);
    }

    public static float loadFloatValue(string key)
    {
        return PlayerPrefs.GetFloat(key, -1f);
    }

    public static float loadIntValue(string key)
    {
        return PlayerPrefs.GetFloat(key, -1);
    }
}
