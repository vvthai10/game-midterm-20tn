using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemiesLayer;

    [SerializeField] private float HitDamage = 30f;

    private Collider2D[] hitEnemies = null;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.Play("attack");
        }
    }

    // event called at the middle of "attack" animation
    public void hit()
    {
        hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesLayer);
        if (hitEnemies != null)
            foreach (Collider2D enemy in hitEnemies)
                enemy.GetComponent<BossHealth>().takeHit(HitDamage);
    }

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
