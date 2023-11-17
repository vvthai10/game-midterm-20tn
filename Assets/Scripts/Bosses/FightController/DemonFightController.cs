using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonFightController : MonoBehaviour
{
    public DemonController demonController;
    public EffectsController effectsController;
    public void Intro()
    {
        effectsController.SetIntroOnReveal(demonController.PlayIntro);
        effectsController.PlayIntroEffect();
    }
}
