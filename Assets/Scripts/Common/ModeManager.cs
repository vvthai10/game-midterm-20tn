using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public static ModeManager instance;

    private GameController gameController = new GameController();
    private MainChararacterStat main = new MainChararacterStat();
    public bool isContinueClick = false;
    private void Awake()
    {
        if(ModeManager.instance != null)
        {
            //Destroy(gameObject);
        }else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
       
    }

    public void SetMode(string mode)
    {
        gameController.mode = mode;
    }

    public void SetScene(int index)
    {
        gameController.sceneIndex = index;
    }

    public string GetMode()
    {
        return gameController.mode;
    }

    public int GetGameScene()
    {
        return gameController.sceneIndex;
    }

    public void SaveCurrentLevel()
    {
        gameController.SaveGameState(GameController.SAVE_PATH);
    }

    public void OnContinueClick()
    {
       
        if (!gameController.LoadData(GameController.SAVE_PATH))
        {
            return;
        }
        FindAnyObjectByType<MainMenu>().PlayGame(GetGameScene());

    }

    public void SetContinueClick(bool isClick)
    {
        isContinueClick = isClick;
    }

    public void SaveMainStat()
    {
        main.LoadStatBeforeSave();
        main.SaveData(MainChararacterStat.SAVE_PATH);
    }

    public void LoadMainStat()
    {
        main.LoadData(MainChararacterStat.SAVE_PATH);
    }

    public MainChararacterStat getMainStat()
    {
        return main;
    }

    public void SetDeathReason(string deathBy)
    {
        main.deathReason = deathBy;
    }
}
[System.Serializable]
public class GameController
{
    public static string SAVE_PATH = "/GameState.json";
    public string mode;
    public int sceneIndex;


    public bool SaveGameState(string relativePath)
    {
        string path = Application.persistentDataPath + relativePath;
        try
        {
            if (File.Exists(path))
            {
                Debug.Log("FIle exist, delete it");
                File.Delete(path);
            }
            else
            {
                Debug.Log("Write new file");
            }
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonUtility.ToJson(this));
            Debug.Log("File path: " + path);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    public bool LoadData(String relativePath)
    {
        string path = Application.persistentDataPath + relativePath;
        if (!File.Exists(path))
        {
            return false;
        }

        try
        {
            GameController data = JsonUtility.FromJson<GameController>(File.ReadAllText(path));
            this.mode = data.mode;
            this.sceneIndex = data.sceneIndex;
        }
        catch (Exception e)
        {
            Debug.LogError("Fail to load data due to: " + e.Message + " " + e.StackTrace);
            return false;
        }
        return true;
    }
}

[System.Serializable]
public class MainChararacterStat
{
    public static string SAVE_PATH = "/MainStat.json";
    /*
     * death by monster/trap -> reset to start location, reset money, stat -> "monster"
     * death by boss -> reset to shop location, remain stat, money -> "boss"
     * save game normally -> remain currently location, remain stat, money -> "none"
     */
    public string deathReason;
    public float maxHealth = 100;
    public float curHealth = 100;
    public float maxStamina = 100;
    public float staminaSpeed = 0.75f;
    public int numberFlask = 5;
    public int souls = 0;
    public float[] position = new float[3] {-5, 2.04f, 0};

    public bool SaveData(string relativePath)
    {
        string path = Application.persistentDataPath + relativePath;
        try
        {
            if (File.Exists(path))
            {
                Debug.Log("FIle exist, delete it");
                File.Delete(path);
            }
            else
            {
                Debug.Log("Write new file");
            }
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonUtility.ToJson(this));
            Debug.Log("File path: " + path);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    public bool LoadData(string relativePath)
    {
        string path = Application.persistentDataPath + relativePath;
        if (!File.Exists(path))
        {
            return false;
        }

        try
        {
            MainChararacterStat data = JsonUtility.FromJson<MainChararacterStat>(File.ReadAllText(path));
            this.SetStat(data);
        }
        catch (Exception e)
        {
            Debug.LogError("Fail to load data due to: " + e.Message + " " + e.StackTrace);
            return false;
        }
        return true;
    }

    public void LoadStatBeforeSave()
    {
        main_character instance = main_character.instance;
        this.maxHealth = instance.healthBar.maxHealth;
        this.curHealth = instance.healthBar.health <= 0 ? instance.healthBar.maxHealth/2 : instance.healthBar.health;
        this.maxStamina = instance.staminaBar.maxStamina;
        this.staminaSpeed = instance.staminaBar.getRegenSpeed();
        this.numberFlask = instance.number_flask;
        this.souls = this.deathReason.ToLower() == "monster" ? instance.oldSouls : instance.souls;
        this.position = new float[3]
        {
            instance.transform.position.x,
            instance.transform.position.y,
            instance.transform.position.z,
        };
    }

    private void SetStat(MainChararacterStat another)
    {
        this.deathReason = another.deathReason;
        this.maxHealth = another.maxHealth;
        this.curHealth = another.curHealth;
        this.maxStamina = another.maxStamina;
        this.staminaSpeed = another.staminaSpeed;
        this.numberFlask = another.numberFlask;
        this.souls = another.souls;
        this.position = another.position;
    }
}