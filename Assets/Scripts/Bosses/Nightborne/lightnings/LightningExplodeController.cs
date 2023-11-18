using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LightningExplodeController : MonoBehaviour
{
    private Animator animator;
    private LightningController parentController;
    private LightningHit hitController;
    void Awake()
    {
        animator = GetComponent<Animator>();
        parentController = GetComponentInParent<LightningController>();
        hitController = GetComponent<LightningHit>();
        this.Hide();
    }

    public void SetAnimatorSpeed(float _speed)
    {
        animator.speed = _speed;
    }

    public void Play()
    {
        this.Show();
        animator.Play("explode");
    }

    public void OnExplodeReveal()
    {
        parentController.OnExplodeReveal();
    }

    public void OnExplodeEnded()
    {
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

    public void SetCanHit(bool _canHit)
    {
        hitController.canHit = _canHit;
    }
}
