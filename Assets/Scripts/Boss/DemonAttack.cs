using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    
    public Transform attackPoint;
    public Transform fireballPoint;
    public float attackRange = 1f;
    public float attackDamage = 20f;
    //public LayerMask playersLayer;
 

    public GameObject[] fireballs;
    public float fireballCooldown = 1;
    private float cooldownTimer = Mathf.Infinity;


    //private Collider2D[] hitPlayers = null;
    private BossGeneral boss;

    private void Start()
    {
        boss = GetComponent<BossGeneral>();
    }

    private void Update()
    {
        if (cooldownTimer > fireballCooldown)
        {
            this.ShootFireball();
        }
        cooldownTimer += Time.deltaTime;
    }

    // event called at the middle of "attack" animation
    public void hit()
    {
        //hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playersLayer);
        //if (hitPlayers != null)
        //    foreach (Collider2D hitPlayer in hitPlayers)
        //    {
        //        main_character.instance.takeDameage(attackDamage);
        //        //hitPlayer.GetComponent<PlayerHealth>().takeHit(attackDamage);
        //    }
        if (this.PlayerIsInAttackRange())
        {
            main_character.instance.takeDameage(attackDamage);
        }
    }

    public bool PlayerIsInAttackRange()
    {
        if (boss.targetedPlayer && Vector2.Distance((Vector2)boss.targetedPlayer.position, (Vector2)attackPoint.position) <= attackRange)
            return true;
        return false;
    }

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void ShootFireball()
    {
        cooldownTimer = 0;
        fireballs[0].transform.position = fireballPoint.position;
        fireballs[0].GetComponent<Fireball>().SetDirection(Mathf.Sign(boss.targetedPlayer.position.x - fireballPoint.position.x));
    }
}
