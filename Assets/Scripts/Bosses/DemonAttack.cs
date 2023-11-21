using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    
    public Transform attackPoint;
    public Transform fireballPoint;
    public float attackRange = 3.5f;
    public float attackDamage = 7f;
    //public LayerMask playersLayer;
 

    public GameObject[] fireballs;
    public float fireballCooldown = 15f;
    private float fireballTimer = 10f;


    public SpikeCastController spikesController;
    public float spikesCooldown = 15f;
    private float spikesTimer = 0;

    public MeteorsController meteorsController;
    public float meteorsCooldown = 15f;
    private float meteorsTimer = 5f;

    //private Collider2D[] hitPlayers = null;
    private BossGeneral boss;



    private void Start()
    {
        boss = GetComponent<BossGeneral>();
    }

    private void Update()
    {
        if (!boss.targetedPlayer)
        {
            return;
        }

        if (fireballTimer > fireballCooldown)
        {
            this.ShootFireball();
            fireballTimer = 0;
        }

        if (spikesTimer > spikesCooldown)
        {
            spikesController?.Cast();
            spikesTimer = 0;
        }

        if (meteorsTimer > meteorsCooldown)
        {
            meteorsController?.Cast();
            meteorsTimer = 0;
        }

        fireballTimer += Time.deltaTime;
        spikesTimer += Time.deltaTime;
        meteorsTimer += Time.deltaTime;
    }

    // event called at the middle of "attack" animation
    public void hit()
    {
        //Debug.Log("Hit called");
        AudioManager.Instance.PlaySFXBossMusic("demon-hit");
        if (this.PlayerIsInAttackRange())
        {
            Debug.Log("Player in attack range");
            try
            {
                //Debug.Log($"Hitting player with damage {attackDamage}");
                main_character.instance.TakeDameage(attackDamage);
                GameManager.instance.SetDeathReason("boss");
                //Debug.Log("Take damage called");
            } catch
            {
                Debug.Log("Error at DemonAttacks.hit");
            }
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
        fireballs[FindFireball()].transform.position = fireballPoint.position;
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
