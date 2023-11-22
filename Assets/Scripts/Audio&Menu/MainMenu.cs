using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuDefault, menuContinue;

    private void Awake() {
        if (CheckIfFolderIsEmpty(Application.persistentDataPath) || CheckIfFolderHasSpecificFile(Application.persistentDataPath, "Player.log"))
        {
            menuDefault.SetActive(true);
            menuContinue.SetActive(false);
        }
        else
        {
            menuDefault.SetActive(false);
            menuContinue.SetActive(true);
        }
    }

    // private void Start() {
    //     Debug.Log("Run start");
    //     if (Directory.Exists(Application.persistentDataPath))
    //     {
    //         menuDefault.SetActive(false);
    //         menuContinue.SetActive(true);
    //     }
    //     else
    //     {
    //         menuDefault.SetActive(true);
    //         menuContinue.SetActive(false);
    //     }
    // }
    
    
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

    bool CheckIfFolderIsEmpty(string path)
    {
        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path);
            string[] directories = Directory.GetDirectories(path);

            if (files.Length == 0 && directories.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    bool CheckIfFolderHasSpecificFile(string path, string fileName)
    {
        if (Directory.Exists(path))
        {
            string[] allFiles = Directory.GetFiles(path);

            if (allFiles.Length == 1 && Path.GetFileName(allFiles[0]) == fileName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
}
