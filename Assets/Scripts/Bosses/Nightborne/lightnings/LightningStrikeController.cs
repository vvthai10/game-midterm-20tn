using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrikeController : MonoBehaviour
{
    private Animator animator;
    private LightningController parentController;
    void Awake()
    {
        animator = GetComponent<Animator>();
        parentController = GetComponentInParent<LightningController>();
        this.Hide();
    }
    public void SetAnimatorSpeed(float _speed)
    {
        animator.speed = _speed;
    }
    public void Play()
    {
        AudioManager.Instance.PlaySFXBossMusic("thunder");
        this.Show();
        animator.Play("strike");
    }

    public void OnStrikeEnded()
    {
        parentController.OnStrikeEnded();
        this.Hide();
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
