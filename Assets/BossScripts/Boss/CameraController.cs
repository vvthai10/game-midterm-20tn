using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    void Update()
    {
        if (playerTransform)
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }
}
