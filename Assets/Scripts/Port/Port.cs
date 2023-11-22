using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Port : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Boolean show = false;
    public static Port instance;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (show)
        {
            if(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / 5;
            }
            else
            {
                show = false;
            }
        
        }
    }

    public void ShowUp()
    {
        gameObject.SetActive(true);
        AudioManager.Instance.PlayBackgroundMusic("Portal");
        show = true;
        //SceneManager.LoadSceneAsync(3);
    }
}
