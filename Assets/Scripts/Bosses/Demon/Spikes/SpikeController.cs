using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpikeController : MonoBehaviour
{
    public float damage = 1f;

    private Animator animator;
    private UnityEvent onSpikeRisen;
    private UnityEvent onSpikeDropped;
    private SpikesController parentController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //onSpikeRisen = new UnityEvent();
        //onSpikeDropped = new UnityEvent();
        //Debug.Log($"{gameObject.name} initializing {onSpikeRisen} {onSpikeDropped}");
        parentController = GetComponentInParent<SpikesController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            parentController.ProcessHit(this.damage);
        }
    }

    public void PlayRiseAnimation()
    {
        animator.SetTrigger("rise");
    }

    public void PlayDropAnimation()
    {
        animator.SetTrigger("drop");
    }

    public void OnSpikeRisen()
    {
        onSpikeRisen.Invoke();
    }

    public void SetOnSpikeRisen(UnityAction action)
    {
        if (onSpikeRisen == null)
        {
            onSpikeRisen = new UnityEvent();
        }
        //Debug.Log($"{gameObject.name} action: " + (action != null).ToString());
        //Debug.Log($"{gameObject.name} onSpikeRisen: " + (onSpikeRisen != null).ToString());
        onSpikeRisen.RemoveAllListeners();
        //Debug.Log($"{gameObject.name} removed listeners");
        onSpikeRisen.AddListener(action);
        //Debug.Log($"{gameObject.name} added listeners");
    }

    public void OnSpikeDropped()
    {
        onSpikeDropped.Invoke();
    }

    public void SetOnSpikeDropped(UnityAction action)
    {
        if (onSpikeDropped == null)
        {
            onSpikeDropped = new UnityEvent();
        }
        onSpikeDropped.RemoveAllListeners();
        onSpikeDropped.AddListener(action);
    }
}
