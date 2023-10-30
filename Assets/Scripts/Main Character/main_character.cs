using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor.Search;
using System.Collections;

public class main_character : MonoBehaviour
{
    // List animation name
    public static string anim_run = "run";
    public static string anim_idle = "idle";
    public static string anim_roll = "roll";
    public static string anim_jump = "jump";
    public static string anim_fall = "fall";
    public static string anim_idle_block = "block";
    public static string anim_attack_one = "attack1";
    public static string anim_attack_two = "attack2";
    public static string anim_attack_three = "attack3";

    List<string> list_bool_anim = new List<string>() { anim_run, anim_idle, anim_roll, anim_jump, anim_idle_block, anim_fall };
    List<string> list_int_anim = new List<string>() { anim_attack_one, anim_attack_two, anim_attack_three };

    public static main_character instance;
    // Attack bool
    public bool canReceiveInput = true;
    public bool inputReceived = false;

    [SerializeField] private LayerMask platformLayerMask; 
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    public GameObject charater;

    float currentMoveValue = 0f;
    float currentJumpValue = 0f;

    private const float moveSpeed = 10.5f;
    private const float jumpSpeed = 30f;

    private const float extraHeight = 0.1f;
    private const float fallHeight = 3f;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        canReceiveInput = true;
        inputReceived = false;
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
            canReceiveInput = true;
        }

        // Jump
        //Debug.Log(isGrounded());
        if (isGrounded() && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            
            currentJumpValue = jumpSpeed;
            setBoolAnimation(anim_jump);
        }

        //Debug.Log(currentMoveValue);
        // Attack 1

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log(canReceiveInput);
            if (canReceiveInput)
            {
                currentMoveValue = 0f;
                inputReceived = true;
                canReceiveInput = false;
            }
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            currentMoveValue = 0f;
            setBoolAnimation(anim_idle_block);
        }

        // Roll
        if (isGrounded() && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            currentMoveValue = moveSpeed;
            if (GetComponent<SpriteRenderer>().flipX)
            {
                currentMoveValue *= -1;
            }
            rigid.velocity = new Vector2(currentMoveValue, rigid.velocity.y + currentJumpValue);
            setBoolAnimation(anim_roll);
        }

        if (rigid.velocity.y < -0f && !isGrounded(fallHeight))
        {
            Debug.Log("Falling: " + rigid.velocity.y);
            setBoolAnimation(anim_fall);
        }

    }

    private bool isGrounded(float height = extraHeight)
    {
       
        RaycastHit2D raycastHit2D = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + height, platformLayerMask);
        Color rayColor;
        bool hitGround = raycastHit2D.collider != null;
        //if (hitGround)
        //{
        //    rayColor = Color.green;
        //    Debug.Log("no hit ground");
        //}
        //else
        //{
        //    rayColor = Color.red;
        //    Debug.Log("hit ground");
        //}
        //Debug.DrawRay(boxCollider.bounds.center, Vector2.down * (boxCollider.bounds.extents.y + height), rayColor);
        return hitGround;
    } 
    

    public void inputManager()
    {
        canReceiveInput = !canReceiveInput;
    }
    private void setAllBoolAnimationOff()
    {
        for (int i = 0; i < list_bool_anim.Count; i++)
        {
            anim.SetBool(list_bool_anim[i], false);
        }
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
