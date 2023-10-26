using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDemo : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();   
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // handle moving left
        if (Input.GetKeyDown(KeyCode.A))
        {
            spriteRenderer.flipX = true;
            animator.SetBool("isRunning", true);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("isRunning", false);
        }

        // handle moving right

        else if (Input.GetKeyDown(KeyCode.D))
        {
            spriteRenderer.flipX= false;
            animator.SetBool("isRunning", true);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("isRunning", false);
        }

        // handle attack
        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("attack");
        }

        // handle hurt
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("hurt");
        }

        // handle death
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            animator.SetTrigger("death");
        }
    }
}
