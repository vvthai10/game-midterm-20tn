using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar instance;

    public Slider healthSlider;
    public Slider regenHealthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    public float health;
    private float lerpSpeed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        IncreaseMaxHealth(0);
        health = maxHealth;
        instance = this;
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
        if(health > maxHealth) {
            health = maxHealth;
        }
    }
    public bool death()
    {
        return health <= 0;
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        health = maxHealth;
        healthSlider.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, maxHealth);
        healthSlider.GetComponent<Slider>().maxValue = maxHealth;
        healthSlider.GetComponent<Slider>().value = maxHealth;
        easeHealthSlider.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, maxHealth);
        easeHealthSlider.GetComponent<Slider>().maxValue = maxHealth;
        easeHealthSlider.GetComponent<Slider>().value = maxHealth;
        regenHealthSlider.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, maxHealth);
        regenHealthSlider.GetComponent<Slider>().maxValue = maxHealth;
        regenHealthSlider.GetComponent<Slider>().value = maxHealth;
    }
}
