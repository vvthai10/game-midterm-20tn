using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    private Animator animator;
    private BossGeneral boss;

    public float MaxHP = 100f;
    private float currentHP;

    public BossHealthBar healthBar;

    private void Start()
    {
        animator = GetComponent<Animator>();
        boss = GetComponent<BossGeneral>();
        currentHP = MaxHP;
        //healthBar = GameObject.FindGameObjectWithTag("BossHealthBar").GetComponent<BossHealthBar>();
    }


    public void takeHit(float hitDamage)
    {
        
        currentHP = Mathf.Max(0, currentHP - hitDamage);

        healthBar.Show();
        healthBar.SetName(boss.bossName);
        healthBar.SetHealth(currentHP / MaxHP);

        if (boss.canEnrage)
        {
            if (!boss.isEnraged && currentHP < 0.5 * MaxHP)
            {
                boss.isEnraged = boss.canEnrage;
                animator.SetTrigger("enrage");
                return;
            }
        }

        if (currentHP > 0)
        {
            animator.Play(boss.isEnraged ? "enrage_hurt" : "hurt");
        }
        else
        {
            animator.Play(boss.isEnraged ? "enrage_death" : "death");
            healthBar.Hide();
        }
        
    }

    // event called at the end of "death" animation
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
