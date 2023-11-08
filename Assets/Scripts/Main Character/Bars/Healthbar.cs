using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider regenHealthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    public float health;
    private float lerpSpeed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value < health)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, health, lerpSpeed);
            regenHealthSlider.value = health;
        }
        else if (healthSlider.value > health)
        {
            regenHealthSlider.value = health;
            healthSlider.value = health;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }
    }

    public void takeDamage(float dmg)
    {
        health -= dmg;
        
    }

    public void heal(float amount)
    {
        health += amount;
        if(health > 100) {
            health = 100;
        }
    }
    public bool death()
    {
        return health <= 0;
    }
}
