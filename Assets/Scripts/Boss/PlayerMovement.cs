using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // experimental
    [SerializeField] private BossHealth targetedMob;

    // configs
    [SerializeField] private float JumpVelocity = 10f;
    [SerializeField] private float RunVelocity = 7f;
    [SerializeField] private bool InitiallyFacingRight = true;

    private enum AnimationStates { idle, run, attack, hurt, death };

    private float dirX = 0;
    private AnimationStates currentState = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rigidbody2d.velocity = new Vector2(dirX * RunVelocity, rigidbody2d.velocity.y);
        UpdateMovementAnimationsState();
        


        if (Input.GetButtonDown("Jump"))
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, JumpVelocity);
        }

        

        animator.SetInteger("state", (int)currentState);
    }

    

    private void UpdateMovementAnimationsState()
    {
        // handle horizontal movement's animations
        if (dirX > 0)
        {
            currentState = AnimationStates.run;
            //spriteRenderer.flipX = !InitiallyFacingRight;
            transform.eulerAngles = new Vector3(0, InitiallyFacingRight ? 0 : 180, 0);
        }
        else if (dirX < 0)
        {
            currentState = AnimationStates.run;
            //spriteRenderer.flipX = InitiallyFacingRight;
            transform.eulerAngles = new Vector3(0, InitiallyFacingRight ? 180 : 0, 0);
        }
        else
        {
            currentState = AnimationStates.idle;
        }
    }

}
