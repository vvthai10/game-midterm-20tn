using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject uiControl;
    public GameObject uiAudio;

    void Start()
    {
        // Ẩn UI khi bắt đầu
        uiControl.SetActive(false);
        uiAudio.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uiControl.activeSelf)
            {
                uiControl.SetActive(false);
                uiAudio.SetActive(false);
                Time.timeScale = 1;
                GameIsPaused = false;
            }
            else
            {
                uiControl.SetActive(true);
                Time.timeScale = 0;
                GameIsPaused = true;
            }
        }
    }

    public void MainMenuView() {
        AudioManager.Instance.ambientSource.Stop();
        AudioManager.Instance.PlayBackgroundMusic("background");;
        SceneManager.LoadSceneAsync(0);
    }
}
