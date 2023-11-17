using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemy_patrol : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    public GameObject limitedStart;
    public GameObject limitedEnd;
    Rigidbody2D rb;
    Animator ator;
    Transform currentPoint;
    enemy_damage enemyDamage;

    private bool faceLimited2;
    private bool isPatrolling;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ator = GetComponent<Animator>();
        enemyDamage = GetComponent<enemy_damage>(); 
        ator.SetBool("isRunning", true);

        currentPoint = limitedEnd.transform;
        faceLimited2 = true;
        isPatrolling = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void patrolliing()
    {
        if (currentPoint == limitedEnd.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        //if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == limited2.transform)
        //{
        //    Flip();
        //}
        //if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == limited1.transform)
        //{
        //    Flip();
        //}
    }

    public void chasingPlayer(Transform playerPos) 
    {
        lookAtPlayer(playerPos);
        Vector2 target = new Vector2(playerPos.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        if(newPos.x > limitedEnd.transform.position.x)
        {
            newPos.x = limitedEnd.transform.position.x;
        } else if(newPos.x < limitedStart.transform.position.x)
        {
            newPos.x = limitedStart.transform.position.x;
        }

        rb.MovePosition(newPos);
    }
    
    public void lookAtPlayer(Transform player)
    {
        if(transform.position.x > player.position.x && faceLimited2)
        {
            Flip();
        }else if(transform.position.x < player.position.x && !faceLimited2)
        {
            Flip();
        }
    }

    public bool onPatrollingState()
    {
        return isPatrolling;
    }

    public void setPatrolState(bool state)
    {
        isPatrolling = state;
    }

    public bool isFaceLimited2()
    {
        return faceLimited2;
    }

    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        faceLimited2 = !faceLimited2;
    }

    private void FixedUpdate()
    {
        if(faceLimited2)
        {
            currentPoint = limitedEnd.transform;
        }else
        {
            currentPoint = limitedStart.transform;
        }
        
        if (enemyDamage.isDeath()) return;
        if (isPatrolling)
        {
            patrolliing();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(limitedStart.transform.position, 0.5f);
        Gizmos.DrawWireSphere(limitedEnd.transform.position, 0.5f);
        Gizmos.DrawLine(limitedStart.transform.position, limitedEnd.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "LimitedBarrier")
        {
            Flip();
        }
    }

}
