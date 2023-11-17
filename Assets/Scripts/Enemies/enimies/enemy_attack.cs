using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_attack : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    Rigidbody2D rb;
    enemy_patrol patrol;
    public float attackRange;
    public Transform centerAttack;
    public RangeAttackType type;
    public float attackWidth;
    public float attackHeight;
    Transform player;
    public static enemy_attack instance;

    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        patrol = GetComponent<enemy_patrol>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public bool inRangeAttack()
    {
        return Vector2.Distance(player.position, transform.position) < attackRange;
    }

    //call in animation event
    //deal damage to user 
    public void attack()
    {

        Collider2D[] colliders;
        if(type == RangeAttackType.Melee || type == RangeAttackType.Range)
        {
            colliders = Physics2D.OverlapCircleAll(centerAttack.position, attackRange);
            string enemyName = gameObject.name.ToLower();
            PlaySFX();
        }
        else
        {
            PlaySFX();
            Vector2 topLeftPoint = new Vector2(centerAttack.position.x, centerAttack.position.y + attackHeight / 2);
            Vector2 bottomRightPoint; 
            if(patrol.isFaceLimited2())
            {
                bottomRightPoint = new Vector2(centerAttack.position.x + attackWidth, centerAttack.position.y - attackHeight / 2);

            } else
            {
                bottomRightPoint= new Vector2(centerAttack.position.x - attackWidth, centerAttack.position.y - attackHeight / 2);
            }
            colliders = Physics2D.OverlapAreaAll(topLeftPoint, bottomRightPoint);
        }

        foreach(Collider2D collider in colliders)
        {
            if(collider.tag == "Player")
            {
                //deal damage to player
                Debug.Log("collider with player");
                main_character.instance.TakeDameage(10f);
            }
        }
    }
    public void PlaySFX()
    {
        string enemyName = gameObject.name.ToLower();
        if (enemyName.Contains("lazer"))
        {
            AudioManager.Instance.PlaySFXLazerMusic("attack");
        }
        else if (enemyName.Contains("puncher"))
        {
            AudioManager.Instance.PlaySFXPuncherMusic("attack");
        }
        else if (enemyName.Contains("archer"))
        {
            AudioManager.Instance.PlaySFXArcherMusic("attack");
        }
    }
}

public enum RangeAttackType
{
    Melee,
    Medium,
    Range
}