using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightborneBehaviour : StateMachineBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    private BossGeneral boss;



    public float speed = 6f;
    public float attackRange = 2f;
    private Animator animator1;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<BossGeneral>();
        animator1 = animator;
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

            if (Vector2.Distance(rb.position, player.position) < attackRange)
            {
                animator.Play("attack");
            }
        }
        else
            animator.Play("idle");

    }

    public void increaseAnimationSpeed(float k)
    {
        animator1.speed = k;   
    }
}
