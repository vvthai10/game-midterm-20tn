using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nightborne : MonoBehaviour
{
    public bool isEnabled = true;
    public NightborneFightController fightController;

    private Collider2D thisCollider;

    private void Awake()
    {
        thisCollider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !isEnabled)
            return;

        TransparentFade.Instance.StartIncrease();

        fightController.Intro();
        thisCollider.enabled = false;
    }
}
