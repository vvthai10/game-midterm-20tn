using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    ModeManager modeManager;
    // Start is called before the first frame update
    void Start()
    {
        modeManager = FindAnyObjectByType<ModeManager>();
        LoadMainStat();
    }

    // Update is called once per frame
    void Update()
    {
        if(main_character.instance.Death())
        {
            SaveMainStat();
            SaveGameCurSceneLevel();
            FindAnyObjectByType<LoadConfig>().SaveEnemiesState();
            FindAnyObjectByType<PauseController>().MainMenuView();
        }
    }

    public void SaveGameCurSceneLevel()
    {
        modeManager.SaveCurrentLevel();
    }

    public void SaveMainStat()
    {
        modeManager.SaveMainStat();
    }

    public void LoadMainStat()
    {
        if(modeManager.isContinueClick)
        {
            modeManager.LoadMainStat();
            SetCharacterStat();
        }
    }

    public void SetCharacterStat()
    {
        main_character instance = main_character.instance;
        MainChararacterStat stat = modeManager.getMainStat();
        instance.number_flask = stat.numberFlask;
        instance.souls = stat.souls;
        instance.healthBar.maxHealth = stat.maxHealth;
        instance.healthBar.IncreaseMaxHealth(0);
        instance.healthBar.health = stat.curHealth;

        instance.staminaBar.maxStamina = stat.maxStamina;
        instance.staminaBar.IncreaseStaminaAmount(0);
        instance.staminaBar.setRegenSpeed(stat.staminaSpeed);

        instance.transform.position = new Vector3(stat.position[0], stat.position[1], stat.position[2]);
    }

    //make for transition scene
    //Create an object in scene 2 
    // call this func in start of that object
    public void LoadMainStateForTransitionScene()
    {
        modeManager.LoadMainStat();
        main_character instance = main_character.instance;
        MainChararacterStat stat = modeManager.getMainStat();
        instance.number_flask = stat.numberFlask;
        instance.souls = stat.souls;
        instance.healthBar.maxHealth = stat.maxHealth;
        instance.healthBar.IncreaseMaxHealth(0);
        instance.healthBar.health = stat.curHealth;

        instance.staminaBar.maxStamina = stat.maxStamina;
        instance.staminaBar.IncreaseStaminaAmount(0);
        instance.staminaBar.setRegenSpeed(stat.staminaSpeed);
    }
}
