using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrajectoryController : MonoBehaviour
{
    private Animator animator;
    private MeteorController parentController;

    public float spinTime = 2f;
    public float speed = 3f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        parentController = GetComponentInParent<MeteorController>();
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

    public void InitAnimationChain()
    {

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
        ContactPoint2D[] contacts = new ContactPoint2D[1];
        collision.GetContacts(contacts);
        parentController.PlayBurnAnimation(new Vector3(transform.position.x, contacts[0].point.y, transform.position.y));
        
    }

}
