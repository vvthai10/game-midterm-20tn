using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Animator animator;
    public float MaxHP = 100f;
    private float currentHP;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHP = MaxHP;
    }


    // Update is called once per frame



    public void takeHit(float hitDamage)
    {
        currentHP = Mathf.Max(0, currentHP - hitDamage);
        if (currentHP > 0)
            animator.Play("hurt");
        else
            animator.Play("death");
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
