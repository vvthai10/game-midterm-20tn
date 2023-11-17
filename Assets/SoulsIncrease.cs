using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulsIncrease : MonoBehaviour
{
    [SerializeField] private CanvasGroup soulsIncreaseCanvas;
    private TextMeshProUGUI _textMeshPro;
    public static SoulsIncrease instance;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private int souls = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        soulsIncreaseCanvas = GetComponent<CanvasGroup>();
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        _textMeshPro.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        _textMeshPro.text = "+ "  + souls.ToString();
        //Debug.Log("Increase: " + _textMeshPro.text);
        if (fadeIn)
        {
            if (soulsIncreaseCanvas.alpha < 1)
            {
                soulsIncreaseCanvas.alpha += Time.deltaTime / 2;
            }
            else
            {
                fadeIn = false;
                HideUI();
            }
        }

        if (fadeOut)
        {
            if (soulsIncreaseCanvas.alpha > 0)
            {

                soulsIncreaseCanvas.alpha -= Time.deltaTime / 2;
            }
            else
            {
                fadeOut = false;
            }
        }
    }

    public void HideUI()
    {
        fadeOut = true;
    }

    public void ShowUI(int amount)
    {
        fadeIn = true;
        souls = amount;
    }
}
