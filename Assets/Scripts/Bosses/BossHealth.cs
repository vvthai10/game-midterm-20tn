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

    public bool regenEnabled = false;
    public float regenWhenUnder = 1f;
    public float regenAfter = 3f;
    private float regenTimer = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boss = GetComponent<BossGeneral>();
        currentHP = MaxHP;
    }

    public void IntroHealthBar()
    {
        //healthBar.ResetFill();
        healthBar.Show();
        //Debug.Log("healthbar nullity: " + (healthBar == null).ToString());
        //Debug.Log("boss nullity: " + (boss == null).ToString());
        //Debug.Log("bossname nullity: " + (boss.bossName == null).ToString());
        healthBar.SetName(boss.bossName);
    }

    private void Update()
    {
        //Debug.Log("Boss health: " + currentHP);
        if (regenEnabled && currentHP / MaxHP < regenWhenUnder)
        {
            if (regenTimer > regenAfter)
            {
                // reset healthbar
                currentHP = healthBar.GetLowerValue() * MaxHP;
                healthBar.ResetFillAt(currentHP / MaxHP);

                regenTimer = 0;
            }
            regenTimer += Time.deltaTime;
        }
    }


    public void takeHit(float hitDamage)
    {
        if (!boss.canTakeHit)
            return;
        if (regenEnabled)
            regenTimer = 0;

        float prevHP = currentHP;
        currentHP = Mathf.Max(0, currentHP - hitDamage);

        healthBar.Show();

        if (healthBar.GetName() != boss.bossName)
        {
            healthBar.ResetFillAt(prevHP / MaxHP);
            healthBar.SetName(boss.bossName);
            healthBar.SetHealth(currentHP / MaxHP);
        }
        else
        {
            healthBar.SetHealth(currentHP / MaxHP);
        }

        if (boss.canEnrage)
        {
            if (!boss.isEnraged && currentHP < 0.5 * MaxHP)
            {
                boss.isEnraged = boss.canEnrage;
                // define enrage behaviour
                if (boss.name.ToLower() == "demon")
                    animator.SetTrigger("enrage");
                else if (boss.name.ToLower() == "nightborne")
                {
                    animator.GetBehaviour<NightborneBehaviour>().speed *= 1.5f;
                    animator.speed = 1.25f;
                }
                return;
            }
        }


        if (currentHP > 0)
        {
            animator.Play(boss.isEnraged && animator.HasState(0, Animator.StringToHash("enrage_hurt")) ? "enrage_hurt" : "hurt");
        }
        else
        {
            animator.Play(boss.isEnraged && animator.HasState(0, Animator.StringToHash("enrage_hurt")) ? "enrage_death" : "death");
            healthBar.Hide();
            TransparentFade.Instance.StartDecrease();
            ShowHideTileMap.Instance.TurnNormal();
        }
        
    }

    public bool IsDeath()
    {
        return currentHP <= 0;
    }


    // event called at the end of "death" animation
    public void DestroySelf()
    {
        BossDeathBanner.instance.ShowUI();
        if (boss.bossName.ToLower() == "demon")
        {
            AudioManager.Instance.PlayBackgroundMusic("Elphael");
        }
        else if (boss.bossName.ToLower() == "nightborne")
        {
            AudioManager.Instance.PlayBackgroundMusic("Caves");
        }
        Destroy(gameObject);
    }
}
