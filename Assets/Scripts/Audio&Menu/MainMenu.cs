using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start() {
        Debug.Log("Show Main menu");
        if(AudioManager.Instance == null) {
            Debug.Log("Not have instance");
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

    public void PlayGame(string type) {
        AudioManager.Instance.bgSource.Stop();
        AudioManager.Instance.PlayAmbientMusic("rain");
        Debug.Log(type);
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
