using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class LightningController : MonoBehaviour
{
    public Transform bossPos;
    public LightningStrikeController lightningStrikeController;
    public LightningExplodeController lightningExplodeController;

    public UnityEvent onExplodeReveal;


    private void Awake()
    {
        transform.position = bossPos?.position ?? transform.position;
    }

    public void Reposition()
    {
        transform.position = bossPos?.position ?? transform.position;
    }

    public void SetAnimatorSpeed(float _speed)
    {
        lightningStrikeController.SetAnimatorSpeed(_speed);
        lightningExplodeController.SetAnimatorSpeed(_speed);
    }

    public void StartAnimationChain()
    {
        Reposition();
        PlayStrikeAnimation();
    }

    public void PlayStrikeAnimation()
    {
        lightningStrikeController.Play();
    }

    public void PlayExplodeAnimation()
    {
        lightningExplodeController.Play();
    }

    public void OnStrikeEnded()
    {
        PlayExplodeAnimation();
    }

    public void OnExplodeReveal()
    {
        onExplodeReveal?.Invoke();
    }

    public void RemoveOnExplodeReveal()
    {
        onExplodeReveal?.RemoveAllListeners();
    }

    public void SetOnExplodeReveal(UnityAction action)
    {
        this.RemoveOnExplodeReveal();
        onExplodeReveal?.AddListener(action);
    }

    public void SetCanHit(bool _canHit)
    {
        lightningExplodeController.SetCanHit(_canHit);
    }
}
