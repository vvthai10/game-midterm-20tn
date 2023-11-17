using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectsController : MonoBehaviour
{
    private DemonIntroEffectController introEffectController;

    private void Awake()
    {
        introEffectController = GetComponentInChildren<DemonIntroEffectController>();
    }

    public void SetIntroOnReveal(UnityAction action)
    {
        introEffectController.onReveal = new UnityEvent();
        introEffectController.onReveal.AddListener(action);
    }

    public void PlayIntroEffect()
    {
        introEffectController.Play();
    }
    
}
