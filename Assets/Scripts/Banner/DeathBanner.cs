using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathBanner : MonoBehaviour
{
    [SerializeField] private CanvasGroup deathBannerCanvas;
    public static DeathBanner instance;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        deathBannerCanvas = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn)
        {
            if(deathBannerCanvas.alpha < 1)
            {
                deathBannerCanvas.alpha += Time.deltaTime;
            }
            else
            {
                fadeIn = false;
            }
        }
        if (fadeOut)
        {
            if (deathBannerCanvas.alpha > 0)
            {

                deathBannerCanvas.alpha -= Time.deltaTime;
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

    public void ShowUI()
    {
        fadeIn = true;
        AudioManager.Instance.PlaySFXMusic("death");
    }
}
