using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowerFillController : MonoBehaviour
{

    private Slider lowerSlider;

    public float FillSpeed = 0.1f;
    public float lowerBoundary = 1;

    void Awake()
    {
        lowerSlider = GetComponent<Slider>();
        ResetFill();
    }

    public float Value()
    {
        return lowerSlider.value;
    }

    public void ResetFill()
    {
        lowerBoundary = 1;
        lowerSlider.value = 1;
    }

    public void ResetFillAt(float hp)
    {
        lowerBoundary = hp;
        lowerSlider.value = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (lowerSlider.value > lowerBoundary)
        {
            lowerSlider.value -= FillSpeed * Time.deltaTime;
        }
    }
}
