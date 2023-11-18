using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    
    public Transform attackPoint;
    public Transform fireballPoint;
    public float attackRange = 1f;
    public float attackDamage = 20f;
    //public LayerMask playersLayer;
 

    public GameObject[] fireballs;
    public float fireballCooldown = 1;
    private float cooldownTimer = Mathf.Infinity;


    public SpikeCastController spikesController;
    public float spikesCooldown = 5f;
    private float spikesTimer = 0;

    //private Collider2D[] hitPlayers = null;
    private BossGeneral boss;

    private void Start()
    {
        boss = GetComponent<BossGeneral>();
    }

    private void Update()
    {
        if (boss.targetedPlayer && cooldownTimer > fireballCooldown)
        {
            this.ShootFireball();
        }

        if (spikesTimer > spikesCooldown)
        {
            spikesController?.Cast();
            spikesTimer = 0;
        }

        cooldownTimer += Time.deltaTime;
        spikesTimer += Time.deltaTime;
    }

    // event called at the middle of "attack" animation
    public void hit()
    {
        if (this.PlayerIsInAttackRange())
        {
            main_character.instance.TakeDameage(attackDamage);
        }
    }

    public bool PlayerIsInAttackRange()
    {
        if (boss.targetedPlayer && Vector2.Distance((Vector2)boss.targetedPlayer.position, (Vector2)attackPoint.position) <= attackRange)
            return true;
        return false;
    }

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void ShootFireball()
    {
        cooldownTimer = 0;
        fireballs[FindFireball()].transform.position = fireballPoint.position;
        //float direction = Mathf.Sign(boss.targetedPlayer.position.x - fireballPoint.position.x);
        fireballs[FindFireball()].GetComponent<Fireball>().SetLocalDirection(-1);
    }

    private int FindFireball()
    {
        for (int i = 0; i< fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy) return i;
        }
        return 0;
    }

}
