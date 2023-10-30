using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor.Search;
using System.Collections;

public class main_character : MonoBehaviour
{
    // List animation name
    static string anim_run = "run";
    static string anim_idle = "idle";
    static string anim_roll = "roll";
    static string anim_jump = "jump";
    static string anim_fall = "fall";
    static string anim_idle_block = "block";
    static string anim_attack = "attack";

    List<string> list_bool_anim = new List<string>() { anim_run, anim_idle, anim_roll, anim_jump, anim_idle_block, anim_fall };
    List<string> list_int_anim = new List<string>() { anim_attack };

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public GameObject charater;

    float currentMoveValue = 0f;
    float currentJumpValue = 0f;

    private const float moveSpeed = 10.5f;
    private const float jumpSpeed = 30f;

    private bool isGrounded; // A flag to track if the character is grounded.

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        setBoolAnimation("idle");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            currentMoveValue = moveSpeed;
            GetComponent<SpriteRenderer>().flipX = false;
            setBoolAnimation(anim_run);
        }

        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            currentMoveValue = -moveSpeed;
            GetComponent<SpriteRenderer>().flipX = true;
            setBoolAnimation(anim_run);
        }
    
        else
        {
            currentMoveValue = 0f;
            setBoolAnimation(anim_idle);
        }

        // Jump
        //isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));

        if ((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            currentJumpValue = jumpSpeed;
            setBoolAnimation(anim_jump);
        }
        
        //Debug.Log(currentMoveValue);
        // Attack 1
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            //currentMoveValue = 0f;
            //setAllBoolAnimationOff();
            //if (anim.GetInteger("attack") == 0)
            //{
            //    anim.SetInteger("attack", 1);
            //}
            //else if (anim.GetInteger("attack") == 1)
            //{
            //    anim.SetInteger("attack", 2);
            //}
            //else if (anim.GetInteger("attack") == 2)
            //{
            //    anim.SetInteger("attack", 3);
            //}
            //else if (anim.GetInteger("attack") == 3)
            //{
            //    anim.SetInteger("attack", 1);
            //}
            //else
            //{
            //    anim.SetInteger("attack", 0);
            //}
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            setBoolAnimation(anim_idle_block);
            currentMoveValue = 0f;
        }

        // Roll
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMoveValue = moveSpeed;
            if (GetComponent<SpriteRenderer>().flipX)
            {
                currentMoveValue *= -1;
            }
            rigid.velocity = new Vector2(currentMoveValue, rigid.velocity.y + currentJumpValue);
            setBoolAnimation(anim_roll);
        }

        if (rigid.velocity.y < -0f)
        {
            Debug.Log("Falling: " + rigid.velocity.y);
            setBoolAnimation(anim_fall);
        }

    }

    void setAllBoolAnimationOff()
    {
        for (int i = 0; i < list_bool_anim.Count; i++)
        {
            anim.SetBool(list_bool_anim[i], false);
        }
        //anim.SetInteger("attack", 0);
    }
    void setBoolAnimation(string animName)
    {
        setAllBoolAnimationOff();
        anim.SetBool(animName, true);
    }
    private void FixedUpdate()
    {
        rigid.velocity = new Vector2(currentMoveValue, rigid.velocity.y + currentJumpValue);
        currentJumpValue = 0;
    }
}
