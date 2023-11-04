using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;
    public Slider easeStaminaSlider;
    public float maxStamina = 100f;
    private float stamina;

    private bool canRegen = true;
    private float regenSpeed = 0.5f;
    private float lerpSpeed = 0.1f;
    private float deltaRestTime = 0.5f;
    private DateTime lastTime;

    public static StaminaBar instance;
    // Start is called before the first frame update
    void Start()
    {
        stamina = maxStamina;
        lastTime = DateTime.Now;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        stamina = Math.Min(maxStamina, stamina);
        if (staminaSlider.value != stamina)
        {
            staminaSlider.value = stamina;
        }

        if (staminaSlider.value != easeStaminaSlider.value)
        {
            easeStaminaSlider.value = Mathf.Lerp(easeStaminaSlider.value, stamina, lerpSpeed);
        }
    }

    public bool loseStamina(float amount, bool regen = false)
    {
        

        //Debug.Log("Lose Stamina: " + amount + "Rest: " + stamina);
        if (amount <= 0)
        {
            setCanRegen(regen);
            if (canRegen)
            {
                stamina -= amount;
                //Debug.Log("Regen...");
            }
            return true;
        }
        else if (amount > 0 && stamina >= amount)
        {
            setCanRegen(regen);
            stamina -= amount;
            return true;
        }
        return false;
    }
    public void setCanRegen(bool value)
    {
        //Debug.Log((DateTime.Now - lastTime).TotalSeconds);
        if (value == true && (DateTime.Now - lastTime).TotalSeconds > deltaRestTime)
        {
            canRegen = value;
        }
        else if (value == false)
        {
            lastTime = DateTime.Now;
            canRegen = value;
        }

    }
    private void FixedUpdate()
    {
        if (canRegen && stamina < maxStamina && stamina < 100)
        {
            //Debug.Log("Regen...");
            stamina = Mathf.Min(maxStamina, stamina + regenSpeed);
        }
    }
}
