using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class enemy_damage : MonoBehaviour
{
    private int souls;
    public float hp = 100;
    private bool deathState = false;
    public HealthBar healthBar;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        System.Random rnd = new System.Random();
        souls = rnd.Next(100, 150);
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Player call this func when attack enemy
    public void TakeDamage(float damage)
    {
        hp -= damage;
        healthBar.takeDamage(damage);
        animator.SetTrigger("isDamage");
        if(hp <= 0)
        {
            deathState = true;
            GetComponent<PolygonCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            animator.SetBool("isDeath", true);
        }
    }
    
    public bool isDeath()
    {
        return deathState;
    }

    public int getSouls()
    {
        return souls;
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
