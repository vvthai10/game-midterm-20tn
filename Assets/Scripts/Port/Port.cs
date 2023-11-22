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

    public GameManager gameManager;
    public ModeManager modeManager;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        modeManager = FindAnyObjectByType<ModeManager>();
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
        //
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            // chuyen man
            gameManager.SetDeathReason("none");
            gameManager.SaveMainStat();
            modeManager.SetScene(3);
            modeManager.SetContinueClick(false);
            SceneManager.LoadSceneAsync(3);
        }

    }
}
