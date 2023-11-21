using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadConfig : MonoBehaviour
{
    public TextAsset configFileEasy;
    public TextAsset configFileHard;
    public List<GameObject> enemiesPool;
    private EnemiesConfig config = null;
    private string gameMode = "easy";
    private SavingEnemies savingEnemies = new SavingEnemies();
    public void LoadConfigFile(string mode)
    {
        mode = mode.ToLower();
        string fileConfig = mode == "easy" ? configFileEasy.text : configFileHard.text;
        config = JsonUtility.FromJson<EnemiesConfig>(fileConfig);
        Debug.Log(config.attack + " " + config.hp +" \n");
        Debug.Log(config.enemies[0]);
    }

    private void Start()
    {
        GetGameModeFromModeManager();
        LoadConfigFile(gameMode);
        SetUpMonster();
        if(FindObjectOfType<ModeManager>().isContinueClick && 
            FindAnyObjectByType<ModeManager>().getMainStat().deathReason.ToLower() != "monster")
        {
            LoadEnemiesState();
            SetUpMonsterWhenLoadAgain();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            SaveEnemiesState();
        }
    }

    //set up monster along with config file 
    private void SetUpMonster()
    {
        for(int i = 0; i < enemiesPool.Count; i++)
        {
            if(config.enemies.Contains(i))
            {
                enemiesPool[i].SetActive(true);
            } else
            {
                enemiesPool[i].SetActive(false);
            }

            enemiesPool[i].GetComponent<enemy_damage>().ConfigHp(config.hp);
            
            if (enemiesPool[i].GetComponent<enemy_attack>().type == RangeAttackType.Range)
            {
                enemiesPool[i].GetComponent<enemy_archer_shot>().attackDamage = config.attack;
            }else
            {
                enemiesPool[i].GetComponent<enemy_attack>().attackDamage = config.attack;
            }
        }
    }
    //save monster cur state before quit game
    public void SaveEnemiesState()
    {
        savingEnemies.LoadEnemiesStateBeforeSave(enemiesPool);
        savingEnemies.isUserSave = true;
        savingEnemies.SaveData(SavingEnemies.SAVE_PATH);
    }

    //Load data monster from file
    private void LoadEnemiesState()
    {
        savingEnemies.LoadData(SavingEnemies.SAVE_PATH);
    }
    //map that data to monster
    private void SetUpMonsterWhenLoadAgain()
    {
        if(savingEnemies.enemies.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < savingEnemies.enemies.Count; i++)
        {
            if (savingEnemies.enemies[i].hp <= 0)
            {
                Destroy(enemiesPool[i]);
            }else
            {
                enemiesPool[i].GetComponent<enemy_damage>().ConfigMonsterHpWhenLoad(savingEnemies.enemies[i].hp);
            }
        }
    }

    public void SetGameMode(string mode)
    {
        this.gameMode = mode.ToLower();
    }

    public void GetGameModeFromModeManager()
    {
        ModeManager gameModeManager = FindObjectOfType<ModeManager>();
        SetGameMode(gameModeManager.GetMode());
    }
}

[System.Serializable]
public class EnemiesConfig
{
    public int hp;
    public float attack;
    public List<int> enemies;
}

[System.Serializable]
public class EnemyInfo
{
    public float hp;
    public EnemyInfo(float hp)
    {
        this.hp = hp;
    }
}

[System.Serializable]
public class SavingEnemies
{   public static string SAVE_PATH = "/GameEnemiesData.json";
    public List<EnemyInfo> enemies = new List<EnemyInfo>();
    public bool isUserSave = false;

    public bool SaveData(string relativePath)
    {
        string path = Application.persistentDataPath + relativePath;
        try
        {
            if(File.Exists(path))
            {
                Debug.Log("FIle exist, delete it");
                File.Delete(path);
            } else
            {
                Debug.Log("Write new file");
            }
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonUtility.ToJson(this));
            Debug.Log("File path: " + path);
            return true;
        } 
        catch(Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    public void LoadData(String relativePath)
    {
        enemies.Clear();
        string path = Application.persistentDataPath + relativePath;
        if(!File.Exists(path))
        {
            return;
        }

        try
        {
            SavingEnemies data  = JsonUtility.FromJson<SavingEnemies>(File.ReadAllText(path));
            this.enemies = data.enemies;
            this.isUserSave = data.isUserSave;
        }  catch (Exception e)
        {
            Debug.LogError("Fail to load data due to: " + e.Message + " " +  e.StackTrace);
        }
    }

    public void LoadEnemiesStateBeforeSave(List<GameObject> enemiesPool)
    {
        enemies.Clear();
        foreach(GameObject enemy in enemiesPool)
        {
            float enemyHp = enemy != null ? enemy.GetComponent<enemy_damage>().hp : 0;
            enemies.Add(new EnemyInfo(enemyHp));
        }
    }

}