using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuDefault, menuContinue;

    private void Awake() {
        Debug.Log("Run again");
        if (Directory.Exists(Application.persistentDataPath))
        {
            menuDefault.SetActive(false);
            menuContinue.SetActive(true);
        }
        else
        {
            menuDefault.SetActive(true);
            menuContinue.SetActive(false);
        }
    }

    private void Start() {
        if (Directory.Exists(Application.persistentDataPath))
        {
            menuDefault.SetActive(false);
            menuContinue.SetActive(true);
        }
        else
        {
            menuDefault.SetActive(true);
            menuContinue.SetActive(false);
        }
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

    public void PlayGame(int index) {
        AudioManager.Instance.bgSource.Stop();
        AudioManager.Instance.PlayAmbientMusic("rain");
        //Debug.Log(type);
        SceneManager.LoadSceneAsync(index);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
