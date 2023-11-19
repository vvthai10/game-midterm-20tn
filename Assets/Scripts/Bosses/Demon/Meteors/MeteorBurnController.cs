using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBurnController : MonoBehaviour
{
    private Animator animator;

    private float burnCooldown = 1f;
    private float burnTimer = 0;
    private bool burning = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (burning)
        {
            if (burnTimer > burnCooldown)
            {
                burnTimer = 0;
            }
            burnTimer += Time.deltaTime;
        }
    }

    public void Reposition(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }

    public void PlayBurnAnimation()
    {
        burnTimer = Mathf.Infinity;
        burning= true;
        animator.SetTrigger("burn");
    }

    public void StopBurnAnimation()
    {
        burning = false;
        animator.SetTrigger("stop");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            main_character.instance.TakeDameage(1f);
        }
    }
}
