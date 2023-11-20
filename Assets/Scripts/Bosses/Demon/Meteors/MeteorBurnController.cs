using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBurnController : MonoBehaviour
{
    public float damage = 1f;

    private Animator animator;

    private float burnCooldown = 1f;
    private float burnTimer = 0;
    private bool burning = false;
    private bool playerIn = false;
    private float burnTime = 3f;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (burning)
        {
            burnTimer += Time.deltaTime;
            if (playerIn && burnTimer > burnCooldown)
            {
                main_character.instance.TakeDameage(damage);
                burnTimer = 0;
            }
        }
    }

    public void Reposition(Vector3 pos)
    {
        transform.position = pos;
    }


    public void PlayBurnAnimation()
    {
        burnTimer = Mathf.Infinity;
        burning = true;
        animator.ResetTrigger("stop");
        animator.SetTrigger("burn");
        this.StopBurnAnimationAfter(burnTime);
    }

    public void SetBurnTime(float seconds)
    {
        burnTime = seconds;
    }

    public void StopBurnAnimation()
    {
        burning = false;
        animator.SetTrigger("stop");
    }

    public void Reset()
    {
        StopAllCoroutines();
        burning = false;
        animator.ResetTrigger("burn");
        animator.ResetTrigger("stop");
        animator.Play("empty");
        burnTimer = 0;
        playerIn = false;
    }

    private void StopBurnAnimationAfter(float seconds)
    {
        StartCoroutine(DelayedStop(seconds));
    }

    private IEnumerator DelayedStop(float seconds)
    {
        //Debug.Log("Waiting....");
        yield return new WaitForSeconds(seconds);
        //Debug.Log("Stopping...");
        this.StopBurnAnimation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerIn = false;
        }
    }

}
