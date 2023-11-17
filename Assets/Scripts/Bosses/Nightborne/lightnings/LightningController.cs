using System;
using System.Collections;
using System.Collections.Generic;
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
    public void StartAnimationChain()
    {
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

}
