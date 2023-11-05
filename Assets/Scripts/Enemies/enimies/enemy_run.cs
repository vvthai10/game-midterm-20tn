using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_run : StateMachineBehaviour
{
    public float speed = 2.5f;

    Transform player;
    Rigidbody2D rb;
    enemy_patrol patrol;
    enemy_attack attack;
    enemy_damage damage;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        patrol = animator.GetComponent<enemy_patrol>();
        attack =  animator.GetComponent<enemy_attack>();
        damage = animator.GetComponent<enemy_damage>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(damage.isDeath())
        {
            return;
        }

        if (patrol.onPatrollingState())
        {
            return;
        }

        patrol.chasingPlayer(player);

        if (attack.inRangeAttack())
        {
            animator.SetTrigger("attacked");
        }

        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("attacked");
    }

}
