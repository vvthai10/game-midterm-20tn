using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Assertions;

public class SpikesController : MonoBehaviour
{
    private SpikeController[] spikeControllers;
    public float delayBetweenSpikes = 0.05f;
    public float delayUntilDrop = 0.5f;


    private bool canHit = false;

    private void Awake()
    {
        spikeControllers = GetComponentsInChildren<SpikeController>();
        //Debug.Log("spikeControllers.Length = " + spikeControllers.Length.ToString());
        Debug.Assert(spikeControllers.Length == 4, "SpikesController needs 4 children SpikeControllers");
        InitAnimationSequence();
    }

    public void PlayAnimationChain()
    {
        this.canHit = true;
        spikeControllers[0].PlayRiseAnimation();
    }
    
    IEnumerator DelayedSpikeRise(int spikeIdx)
    {
        yield return new WaitForSeconds(delayBetweenSpikes);
        //Debug.Log("Playing " +  spikeIdx.ToString());
        spikeControllers[spikeIdx]?.PlayRiseAnimation();
    }
    
    IEnumerator DelayedSpikeDrop(int spikeIdx)
    {
        yield return new WaitForSeconds(delayBetweenSpikes);
        spikeControllers[spikeIdx]?.PlayDropAnimation();
    }

    IEnumerator DelayedDropping()
    {
        yield return new WaitForSeconds(delayUntilDrop);
        spikeControllers[spikeControllers.Length - 1].PlayDropAnimation();
    }
    
    private void InitAnimationSequence()
    {
        for (int i = 0; i < spikeControllers.Length - 1; i++)
        {
            int next = i + 1;
            //Debug.Log("spikeControllers[i] = " + spikeControllers[i].ToString());
            spikeControllers[i].SetOnSpikeRisen(() =>
            {
                StartCoroutine(DelayedSpikeRise(next));
            });
        }

        spikeControllers[spikeControllers.Length - 1].SetOnSpikeRisen(() =>
        {
            StartCoroutine(DelayedDropping());
        });

        for (int i = spikeControllers.Length - 1; i > 0; i--)
        {
            int prev = i - 1;
            spikeControllers[i].SetOnSpikeDropped(() =>
            {
                StartCoroutine(DelayedSpikeDrop(prev));
            });
        }

        spikeControllers[0].SetOnSpikeDropped(() =>
        {
            this.canHit = false;
        });
    }

    public void ProcessHit(float damage)
    {
        if (canHit)
        {
            main_character.instance.TakeDameage(damage);
            canHit = false;
        }
    }

}
