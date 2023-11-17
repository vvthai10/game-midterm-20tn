using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadConfig : MonoBehaviour
{
    public TextAsset configFileEasy;
    public TextAsset configFileHard;
    private EnemiesConfig config = null;
    public List<GameObject> enemiesPool;
    public void LoadConfigFile()
    {
        config = JsonUtility.FromJson<EnemiesConfig>(configFileEasy.text);
        Debug.Log(config.attack + " " + config.hp +" \n");
        Debug.Log(config.enemies[0]);
    }

    private void Start()
    {
        LoadConfigFile();
        SetUpMonster();
    }

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

            enemiesPool[i].GetComponent<enemy_damage>().hp = config.hp;
            
            if (enemiesPool[i].GetComponent<enemy_attack>().type == RangeAttackType.Range)
            {
                enemiesPool[i].GetComponent<enemy_archer_shot>().attackDamage = config.attack;
            }else
            {
                enemiesPool[i].GetComponent<enemy_attack>().attackDamage = config.attack;
            }
        }
    }
}

[System.Serializable]
public class EnemiesConfig
{
    public int hp;
    public float attack;
    public List<int> enemies;
}