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
    // Start is called before the first frame update
    void Start()
    {
        targetedPlayer = GameObject.FindGameObjectWithTag("Player").transform;
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
