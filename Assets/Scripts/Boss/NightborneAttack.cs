using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightborneAttack : MonoBehaviour
{
    public Transform attackPoint;
    //public Transform fireballPoint;
    public float attackRange = 1f;
    public float attackDamage = 20f;

    //public GameObject[] fireballs;
    //public float fireballCooldown = 1;
    //private float cooldownTimer = Mathf.Infinity;


    //private Collider2D[] hitPlayers = null;
    private BossGeneral boss;

    private void Start()
    {
        boss = GetComponent<BossGeneral>();
    }

    // event called at the middle of "attack" animation
    public void hit()
    {
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
}
