using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlOptions : MonoBehaviour
{
    public static ControlOptions Instance;
    public GameObject uiPanel;

    void Awake(){
        Instance = this;
    }

    public bool CheckOpen() {
        return uiPanel.active;
    }

    void Start()
    {
        // Ẩn UI khi bắt đầu
        uiPanel.SetActive(false);
    }
    
    // private void OnEnable()
    // {
    //     Debug.Log("OnEnable");
    //     Debug.Log(AudioManager.Instance.defaultMusicStart);
    //     if (AudioManager.Instance.defaultMusicStart == "background") {
    //         AudioManager.Instance.PlayBackgroundMusic("background");
    //     } else {
    //         AudioManager.Instance.PlayAmbientMusic("rain");
    //     }
    // }

    // private void OnDisable()
    // {
    //     Debug.Log("OnDisable");
    //     Debug.Log(AudioManager.Instance.defaultMusicStart);
    //     if (AudioManager.Instance.defaultMusicStart == "background") {
    //         AudioManager.Instance.bgSource.clip = null;
    //     } else {
    //         AudioManager.Instance.ambientSource.clip = null;
    //     }
    // }

    void Update()
    {
        // Kiểm tra nút Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Nếu UI đang hiển thị, ẩn đi; ngược lại, hiển thị UI
            if (uiPanel.activeSelf)
            {
                uiPanel.SetActive(false);
            }
            else
            {
                uiPanel.SetActive(true);
            }
        }
    }

    public void MainMenuView() {
        AudioManager.Instance.ambientSource.Stop();
        AudioManager.Instance.PlayBackgroundMusic("background");;
        SceneManager.LoadSceneAsync(0);
    }
}
