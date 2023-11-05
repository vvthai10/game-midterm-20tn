using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_archer_shot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject arrowPrefab; 

    public void Shoot()
    {
        //Debug.Log("shoot");
        Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
    }
}
