using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor.Search;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

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
    public static string anim_combo_one = "combo1";

    List<string> list_bool_anim = new List<string>() { anim_run, anim_idle, anim_roll, anim_jump, anim_idle_block, anim_fall };
    List<string> list_int_anim = new List<string>() { anim_attack_one, anim_attack_two, anim_attack_three, anim_combo_one };

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

    private float currentMoveValue = 0f;
    private float currentJumpValue = 0f;

    private float currentDistanceRoll = 0f;

    private const float distanceRoll = 7.5f;
    private const float moveSpeed = 10f;
    private const float jumpSpeed = 30f;
    private const float rollSpeed = 20f;
    private const float boostSpeed = 1.25f;
    private const float slowSpeed = 0.75f;
    private const float normalSpeed = 1.0f;

    private const float extraHeight = 0.1f;
    private const float notFallHeight = 3f;

    public static bool finishRoll = true;

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
        finishRoll = true;
        setBoolAnimation("idle");
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.D) /*&& !Input.GetKeyDown(KeyCode.Space)*/)
        {
            spriteRenderer.flipX = false;
            if (finishRoll)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {

                    anim.speed = boostSpeed;
                    currentMoveValue = moveSpeed * boostSpeed;
                }
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                    anim.speed = slowSpeed;
                    currentMoveValue = moveSpeed * slowSpeed;
                }
                else
                {
                    anim.speed = normalSpeed;
                    currentMoveValue = moveSpeed * normalSpeed;
                }

                setBoolAnimation(anim_run);
            }
        }

        else if (Input.GetKey(KeyCode.A)/* && !Input.GetKeyDown(KeyCode.Space)*/)
        {
            spriteRenderer.flipX = true;
            if (finishRoll)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    anim.speed = boostSpeed;
                    currentMoveValue = -moveSpeed * boostSpeed;
                }
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                    anim.speed = slowSpeed;
                    currentMoveValue = -moveSpeed * slowSpeed;
                }
                else
                {
                    anim.speed = normalSpeed;
                    currentMoveValue = -moveSpeed * normalSpeed;
                }
                setBoolAnimation(anim_run);
            }
        }
        else
        {
            if (finishRoll)
            {
                currentMoveValue = 0;
                setBoolAnimation(anim_idle);
                canReceiveInput = true;
            }
            
        }

        // Jump
        //Debug.Log(isGrounded());
        if (isGrounded() && (Input.GetKeyDown(KeyCode.F)))
        {
            currentJumpValue = jumpSpeed;
            setBoolAnimation(anim_jump);
        }

        // Roll
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(rollTo());
        }

        //Debug.Log(currentMoveValue);
        // Attack 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log(canReceiveInput);
            if (canReceiveInput)
            {
                currentMoveValue = 0f;
                inputReceived = true;
                canReceiveInput = false;
            }
        }
        // Block
        if (Input.GetKey(KeyCode.Mouse1))
        {
            currentMoveValue = 0f;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                setAllBoolAnimationOff();
                anim.SetTrigger(anim_combo_one);
            }
            else
            {
                setBoolAnimation(anim_idle_block);
            }
        }



        if (rigid.velocity.y < -0f && !isGrounded(notFallHeight))
        {
            Debug.Log("Falling: " + rigid.velocity.y);
            setBoolAnimation(anim_fall);
        }
        //Debug.Log("Speed anim: " + anim.speed + " / Move speed: " + currentMoveValue);
        
    }

    private bool isGrounded(float height = extraHeight)
    {
       
        RaycastHit2D raycastHit2D = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + height, platformLayerMask);
        bool hitGround = raycastHit2D.collider != null;
        //Color rayColor;
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


    IEnumerator rollTo()
    {
        if (spriteRenderer.flipX)
        {
            currentDistanceRoll = -distanceRoll;
        }
        else
        {
            currentDistanceRoll = distanceRoll;
        }
        float targetX = transform.position.x + currentDistanceRoll;
        setBoolAnimation(anim_roll);
        finishRoll = false;
        while (!finishRoll)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), rollSpeed * Time.deltaTime);
            finishRoll = (Math.Abs(transform.position.x - targetX) <= 0.1f);
            Debug.Log("X: " + transform.position.x   + "v: " + rollSpeed * Time.deltaTime + "Delta roll: " + Math.Abs(transform.position.x - targetX) + " Should stop roll: " + finishRoll);

            yield return null;
        }
        
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
        Debug.Log("Finish roll: " + finishRoll);
        if (finishRoll)
        {
            rigid.velocity = new Vector2(currentMoveValue, rigid.velocity.y + currentJumpValue);
        }

        //targetX = transform.position.x;
        //targetY = transform.position.y;
        currentJumpValue = 0;
    }
}
