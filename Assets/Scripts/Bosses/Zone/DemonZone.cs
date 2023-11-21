using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonZone : MonoBehaviour
{
    public bool isEnabled = true;
    public DemonFightController fightController;

    private Collider2D thisCollider;

    private void Awake()
    {
        thisCollider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !isEnabled)
            return;

        try
        {
            TransparentFade.Instance.StartIncrease();
            ShowHideTileMap.Instance.TurnBoss();
        }
        catch { }

        fightController.Intro();
        thisCollider.enabled = false;
    }
}
