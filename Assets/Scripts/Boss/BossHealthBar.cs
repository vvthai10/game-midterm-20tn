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


    private void Awake()
    {
        upperSlider = GetComponent<Slider>();
        lowerFill = GetComponentInChildren<LowerFillController>();
        bossName = GetComponentInChildren<Text>();
        ResetFill();
        this.Hide();
    }

    public float GetLowerValue()
    {
        return lowerFill.Value();
    }

    public void ResetFill()
    {
        upperSlider.value = 1;
        lowerFill.ResetFill();
    }

    public void ResetFillAt(float hp)
    {
        upperSlider.value = hp;
        lowerFill.ResetFillAt(hp);
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

    public string GetName()
    {
        return (string)bossName.text;
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
