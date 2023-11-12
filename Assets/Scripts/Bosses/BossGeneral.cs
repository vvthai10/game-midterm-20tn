using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGeneral : MonoBehaviour
{
    public Transform targetedPlayer;
    public string bossName;
    public bool initiallyFacingRight;
    public bool isEnraged = false;
    public bool canEnrage = false;
    public AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        targetedPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        if (bossName.ToLower() == "demon")
        {
            audioManager.PlayBackgroundMusic("Boss 2");
        } else if (bossName.ToLower() == "nightborne")
        {
            audioManager.PlayBackgroundMusic("Boss 1");
        }
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

}
