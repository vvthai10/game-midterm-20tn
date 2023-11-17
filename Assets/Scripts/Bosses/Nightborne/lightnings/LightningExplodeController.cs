using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningExplodeController : MonoBehaviour
{
    private Animator animator;
    private LightningController parentController;
    void Awake()
    {
        animator = GetComponent<Animator>();
        parentController = GetComponentInParent<LightningController>();
        this.Hide();
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
}
