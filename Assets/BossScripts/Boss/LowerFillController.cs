using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowerFillController : MonoBehaviour
{

    private Slider lowerSlider;

    public float FillSpeed = 0.1f;
    public float lowerBoundary = 1;

    void Start()
    {
        lowerSlider = GetComponent<Slider>();
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
