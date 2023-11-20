using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrajectoryController : MonoBehaviour
{
    private Animator animator;
    private MeteorController parentController;

    public float spinTime = 1.5f;
    public float speed = 6f;
    public float damage = 3f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        parentController = GetComponentInParent<MeteorController>();
    }

    public void Reset()
    {
        animator.ResetTrigger("spin");
        animator.ResetTrigger("fly");
        animator.Play("empty");
    }

    public void RepositionY(float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
    public void PlaySpinAnimation()
    {
        animator.SetTrigger("spin");
    }

    public void PlayFlyAnimation()
    {
        animator.SetTrigger("fly");
    }

    IEnumerator DelayedFlyAnimation()
    {
        yield return new WaitForSeconds(spinTime);
        PlayFlyAnimation();
    }

    public void PlayExplodeAnimation()
    {
        animator.SetTrigger("explode");
    }

    public void PlayAnimationChain()
    {
        this.PlaySpinAnimation();
        StartCoroutine(DelayedFlyAnimation());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"hit something: {collision.gameObject.name}");
        this.PlayExplodeAnimation();
        if (collision.CompareTag("Player"))
        {
            main_character.instance.TakeDameage(damage);
        }
        else if (collision.CompareTag("Ground"))
        {
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            collision.GetContacts(contacts);
            parentController.PlayBurnAnimation(contacts[0].point);
        }
    }

}
