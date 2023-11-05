using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_patrol : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    public GameObject limited1;
    public GameObject limited2;
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

        currentPoint = limited2.transform;
        faceLimited2 = true;
        isPatrolling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyDamage.isDeath()) return;
        if(isPatrolling)
        {
            patrolliing();
        }
    }

    void patrolliing()
    {
        if (currentPoint == limited2.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == limited2.transform)
        {
            Flip();
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == limited1.transform)
        {
            Flip();
        }
    }

    public void chasingPlayer(Transform playerPos) 
    {
        lookAtPlayer(playerPos);
        Vector2 target = new Vector2(playerPos.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        if(newPos.x > limited2.transform.position.x)
        {
            newPos.x = limited2.transform.position.x;
        } else if(newPos.x < limited1.transform.position.x)
        {
            newPos.x = limited1.transform.position.x;
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

    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        faceLimited2 = !faceLimited2;
    }

    private void FixedUpdate()
    {
        if(faceLimited2)
        {
            currentPoint = limited2.transform;
        }else
        {
            currentPoint = limited1.transform;
        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(limited1.transform.position, 0.5f);
        Gizmos.DrawWireSphere(limited2.transform.position, 0.5f);
        Gizmos.DrawLine(limited1.transform.position, limited2.transform.position);
    }
}
