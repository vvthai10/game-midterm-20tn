using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NightborneFightController : MonoBehaviour
{
    private NightborneController nightborneController;
    private LightningController lightningController;
    private void Awake()
    {
        nightborneController = GetComponentInChildren<NightborneController>();  
        lightningController = GetComponentInChildren<LightningController>();
    }

    public void Intro()
    {
        lightningController.SetAnimatorSpeed(1.0f);
        lightningController.SetOnExplodeReveal(nightborneController.ShowBoss);
        lightningController.StartAnimationChain();
    }

    public void PlayRawLightningAnimation()
    {
        lightningController.SetAnimatorSpeed(2.0f);
        lightningController.RemoveOnExplodeReveal();
        lightningController.StartAnimationChain();
    }
}
