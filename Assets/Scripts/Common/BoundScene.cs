using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoundScene : MonoBehaviour
{
    public GameManager gameManager;
    public ModeManager modeManager;

    private void Start()
    {
        modeManager = FindAnyObjectByType<ModeManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            // chuyen man
            gameManager.SaveMainStat();
            modeManager.SetScene(2);
            modeManager.SetContinueClick(false);
            SceneManager.LoadSceneAsync(2);
           
        }
    }
}
