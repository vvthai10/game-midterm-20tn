using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_attack : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    Rigidbody2D rb;
    public float attackRange;
    public Transform centerAttack;
    public RangeAttackType type;
    public float attackWidth;
    public float attackHeight;
    Transform player;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
        }else
        {
           
            Vector2 topLeftPoint = new Vector2(centerAttack.right.x, centerAttack.right.y + attackHeight / 2);
            Vector2 bottomRightPoint = new Vector2(centerAttack.right.x + attackWidth, centerAttack.right.y - attackHeight / 2);
 
            colliders = Physics2D.OverlapAreaAll(topLeftPoint, bottomRightPoint);
        }

        foreach(Collider2D collider in colliders)
        {
            if(collider.tag == "Player")
            {
                //deal damage to player
                Debug.Log("collider with player");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(type == RangeAttackType.Melee)
        {
            Gizmos.DrawWireSphere(transform.position, attackRange);
        } else
        {
            Gizmos.DrawLineStrip(
                new Vector3[4]
                {
                    new Vector3(centerAttack.position.x, centerAttack.position.y + attackHeight / 2, 0),
                    new Vector3(centerAttack.position.x + attackWidth, centerAttack.position.y + attackHeight / 2, 0),
                    new Vector3(centerAttack.position.x + attackWidth, centerAttack.position.y - attackHeight / 2, 0),
                    new Vector3(centerAttack.position.x, centerAttack.position.y - attackHeight / 2, 0)
                },
                true
            );
        }
       
    }
}

public enum RangeAttackType
{
    Melee,
    Medium,
    Range
}