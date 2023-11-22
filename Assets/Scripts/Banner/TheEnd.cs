using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEnd : MonoBehaviour
{
    [SerializeField] private CanvasGroup endBannerCanvas;
    public static TheEnd instance;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private DateTime startDelay;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        endBannerCanvas = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn)
        {
            if (endBannerCanvas.alpha < 1)
            {
                endBannerCanvas.alpha += Time.deltaTime / 1.5f;
            }
            else
            {
                fadeIn = false;
                HideUI();
            }
        }

        if (fadeOut && (DateTime.Now - startDelay).TotalSeconds >= 1f)
        {
            if (endBannerCanvas.alpha > 0)
            {

                endBannerCanvas.alpha -= Time.deltaTime / 5f;
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
        startDelay = DateTime.Now;
    }

    public void ShowUI()
    {
        fadeIn = true;
        AudioManager.Instance.PlaySFXBossMusic("death");
    }
}
