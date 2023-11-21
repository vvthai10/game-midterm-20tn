using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : MonoBehaviour
{
    private BossGeneral boss;
    private BossHealth health;
    private Animator animator;

    private void Awake()
    {
        boss = GetComponent<BossGeneral>();
        health = GetComponent<BossHealth>();
        animator = GetComponent<Animator>();
    }

    public void PlayIntro()
    {
        //this.Show();
        boss.Show();
        boss.LookAtPlayer();
        health.IntroHealthBar();
        StartCoroutine(StartAttackingAfter(0.5f));
    }

    IEnumerator StartAttackingAfter(float seconds = 0)
    {
        yield return new WaitForSeconds(seconds);
        animator.SetTrigger("start");
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
