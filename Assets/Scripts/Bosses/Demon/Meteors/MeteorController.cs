using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    private TrajectoryController trajectoryController;
    private MeteorBurnController burnController;
    public float trajectoryHeight = 8f;
    public float burnTime = 3f;

    private void Awake()
    {
        trajectoryController = GetComponentInChildren<TrajectoryController>();
        burnController = GetComponentInChildren<MeteorBurnController>();
    }
    public void Cast()
    {
        burnController.Reset();
        burnController.SetBurnTime(burnTime);
        trajectoryController.Reset();
        trajectoryController.RepositionY(transform.position.y + trajectoryHeight);
        trajectoryController.PlayAnimationChain();
    }

    public void SetTrajectoryHeight(float h)
    {
        trajectoryHeight = h;
    }

    public void RepositionX(float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    public void PlayBurnAnimation(Vector2 pos)
    {   
        burnController.Reposition(new Vector3(transform.position.x, pos.y, transform.position.z));
        burnController.PlayBurnAnimation();
    }
    
}
