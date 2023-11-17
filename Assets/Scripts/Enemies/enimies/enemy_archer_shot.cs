using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_archer_shot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject arrowPrefab;
    public float attackDamage = 20f;

    public void Shoot()
    {
        //Debug.Log("shoot");
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        arrow.GetComponent<arrow>().SetAttackDamage(attackDamage);
    }
}
