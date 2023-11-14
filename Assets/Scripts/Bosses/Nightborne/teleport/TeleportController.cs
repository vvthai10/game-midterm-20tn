using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportController : MonoBehaviour
{
    private Animator animator;
    public UnityEvent onDisappear; // referring to NightborneController.OnDisappear
    public UnityEvent onAppear; // referring to NightborneController.OnAppear
    public float teleportDelay = 1.0f;
    public Transform bossTransform;

    public void Reposition()
    {
        transform.position = bossTransform?.position ?? transform.position;
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAppearAnimation()
    {
        Reposition();
        animator.Play("appear");
    }

    // This animation calls OnDisappeared at the end
    public void PlayDisappearAnimation()
    {
        Reposition();
        animator.Play("disappear");
    }

    // called at the end of "disappear" animation
    public void OnDisappear()
    {
        onDisappear?.Invoke();
    }

    public void OnAppear()
    {
        onAppear?.Invoke();
    }
}
