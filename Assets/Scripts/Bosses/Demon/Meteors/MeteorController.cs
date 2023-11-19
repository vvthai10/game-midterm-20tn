using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    public TrajectoryController trajectoryController;
    public MeteorBurnController burnController;

    public void Cast()
    {
        trajectoryController.PlayAnimationChain();

    }

    public void PlayBurnAnimation(Vector3 pos)
    {
        burnController.Reposition(pos);
        burnController.PlayBurnAnimation();
    }
}
