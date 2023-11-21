using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DemonIntroEffectController : MonoBehaviour
{
    private Animator animator;
    public UnityEvent onReveal;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Play()
    {
        animator.Play("intro");
        AudioManager.Instance.PlaySFXBossMusic("demon-intro");
    }

    public void OnReveal()
    {
        onReveal?.Invoke();
    }
}
