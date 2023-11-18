using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeCastController : MonoBehaviour
{
    private SpikesController forwardSpikesController;
    public Transform target;
    public BossGeneral boss;
    public float offsetFromBoss = 4;

    private void Awake()
    {
        forwardSpikesController = GetComponentInChildren<SpikesController>();
    }

    private void Reposition()
    {
        float bossDirection = boss.Direction();

        if (bossDirection > 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);

        if (target)
        {
            transform.position = new Vector3(target.position.x + bossDirection * offsetFromBoss, transform.position.y, transform.position.z);
        }
    }

    public void Cast()
    {
        Reposition();
        forwardSpikesController.PlayAnimationChain();
    }
}
