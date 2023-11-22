using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossGeneral : MonoBehaviour
{
    public Transform targetedPlayer;
    
    public string bossName;
    public bool initiallyFacingRight;
    public bool isEnraged = false;
    public bool canEnrage = false;
    public AudioManager audioManager;
    private Dictionary<string, int> souls = new Dictionary<string, int>() {
        { "demon", 2000},
        { "nightborne", 1000 }
    };

    public bool canTakeHit = true;


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        targetedPlayer = GameObject.FindGameObjectWithTag("Player").transform;

        this.Hide();
    }

    public void LookAtPlayer()
    {
        if (transform.position.x > targetedPlayer.position.x)
        {
            transform.eulerAngles = new Vector3(0, initiallyFacingRight ? 180 : 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, initiallyFacingRight ? 0 : 180, 0);
        }
    }

    public int Direction()
    {
        int dir = initiallyFacingRight ? 1 : -1;
        dir = transform.eulerAngles.y == 0 ? dir : -dir;
        return dir;
    }

    public int getSouls(string bossName)
    {
        return souls[bossName.ToLower()];
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        Debug.Log("Boss name: " + bossName.ToLower());
        if (bossName.ToLower() == "demon")
        {
            AudioManager.Instance.PlayBackgroundMusic("Boss 2");
            AudioManager.Instance.PlayAmbientMusic("fire");
        }
        else if (bossName.ToLower() == "nightborne")
        {
            AudioManager.Instance.PlayBackgroundMusic("Boss 1");
            AudioManager.Instance.PlayAmbientMusic("rain and thunder");
        }
        gameObject.SetActive(true);
    }
    
}
