using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_damage : MonoBehaviour
{
    public float hp = 100;
    private bool deathState = false;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("key click");
            TakeDamage(20);
        }
    }

    //Player call this func when attack enemy
    public void TakeDamage(int damage)
    {
        hp -= damage;
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

    public void Die()
    {
        Destroy(gameObject);
    }
}
