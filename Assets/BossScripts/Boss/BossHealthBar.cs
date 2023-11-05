using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider upperSlider;
    public Text bossName;
    private LowerFillController lowerFill;

    private void Start()
    {
        Debug.Log("Boss health bar started");
        upperSlider = GetComponent<Slider>();
        lowerFill = GetComponentInChildren<LowerFillController>();
        bossName = GetComponentInChildren<Text>();
        this.Hide();
    }

    public void SetHealth(float health)
    {
        upperSlider.value = health;
        lowerFill.lowerBoundary = health;
    }

    public void SetName(string name)
    {
        bossName.text = name;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
