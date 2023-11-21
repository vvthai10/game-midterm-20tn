using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NightborneAttack : MonoBehaviour
{
    public Transform attackPoint;
    //public Transform fireballPoint;
    public float attackRange = 1f;
    public float attackDamage = 20f;

    // skills
    public float teleportCooldown = 10f;
    private float teleportTimer = 0f;

    private BossGeneral boss;
    private Rigidbody2D rb;
    private Animator animator;
    private NightborneController controller;
    private BossHealth health;
    

    private void Awake()
    {
        boss = GetComponent<BossGeneral>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controller = GetComponent<NightborneController>();
        health = GetComponent<BossHealth>();
    }

    private void Update()
    {
        if (!boss.targetedPlayer || health.IsDeath())
            return;

        if (Vector2.Distance(rb.position, boss.targetedPlayer.position) < attackRange)
        {
            animator.Play("attack");
        }


        teleportTimer += Time.deltaTime;
        if (teleportTimer >  teleportCooldown)
        {
            this.Teleport();
            teleportTimer = 0;
        }
    }

    // event called at the middle of "attack" animation
    public void hit()
    {
        AudioManager.Instance.PlaySFXBossMusic("nightborne-hit");
        if (this.PlayerIsInAttackRange())
        {
            GameManager.instance.SetDeathReason("boss");
            main_character.instance.TakeDameage(attackDamage);
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



    public void Teleport()
    {
        controller.BeginTeleportChain(); // NightborneController
    }
}
