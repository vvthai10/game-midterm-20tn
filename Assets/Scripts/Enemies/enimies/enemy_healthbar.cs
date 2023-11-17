using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy_healthbar : MonoBehaviour
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
        IncreaseMaxHealth(0);
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
        Debug.Log("Current health: " + health);
    }

    private void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;

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

    public void configMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
        IncreaseMaxHealth(0);
    }
}
