using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBehaviour : StateMachineBehaviour
{

    private Transform player;
    private Rigidbody2D rb;
    private BossGeneral boss;
    private BossAttack bossAttack;


    [SerializeField] private float speed = 3f;
    //[SerializeField] private float attackRange = 2f;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<BossGeneral>();
        bossAttack = animator.GetComponent<BossAttack>();   
        player = boss.targetedPlayer;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player)
        {
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPosition = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            boss.LookAtPlayer();
            AudioManager.Instance.PlaySFXBossMusic("demon-run");
            //if (Vector2.Distance(rb.position, player.position) < attackRange)
            //{
            //    
            //    animator.Play(boss.isEnraged ? "enrage_attack" : "attack");
            //}

            if (bossAttack.PlayerIsInAttackRange())
            {
                //Debug.Log("Boss found player in attack range");
                animator.Play(boss.isEnraged ? "enrage_attack" : "attack");
            }
        }
        else
            animator.Play("idle");
        
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
