using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningHit : MonoBehaviour
{
    public bool canHit = false;
    public float damage = 5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canHit && collision.gameObject.CompareTag("Player"))
        {
            main_character.instance.TakeDameage(damage);
            canHit = false;
        }
    }
}
