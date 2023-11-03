using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor.Search;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class main_character : MonoBehaviour
{
    public int number_flask;
    public const int max_flask = 5;
    private float amount_health_heal = 0.2f;

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
    public static string anim_hurt = "hurt";
    public static string anim_wall_slide = "wall_slide";

    // List stamina cost
    public static string stamina_run_boost = "run--boost";
    public static string stamina_run_slowed = "run--slowed";
    public static string stamina_run_normal = "run";
    public static string stamina_idle = "idle";
    public static string stamina_roll = "roll";
    public static string stamina_jump = "jump";
    public static string stamina_fall = "fall";
    public static string stamina_idle_block = "block";
    public static string stamina_attack = "attack";
    public static string stamina_wall_slide = "wall_slide";

    List<string> list_bool_anim = new List<string>() { anim_run, anim_idle, anim_roll, anim_jump, anim_idle_block, anim_fall, anim_wall_slide };
    List<string> list_int_anim = new List<string>() { anim_attack_one, anim_attack_two, anim_attack_three, anim_combo_one };
    Dictionary<string, float> stamina_amount = new Dictionary<string, float>() {
        {stamina_run_boost, 0.1f },
        {stamina_run_normal, 0f},
        {stamina_run_slowed, -0.01f},
        {stamina_roll, 20f },
        {stamina_jump, 15f},
        {stamina_attack, 25f },
        {stamina_idle, -0.025f },
        {stamina_fall, 0f },
        {stamina_idle_block, 0.1f },
        {stamina_wall_slide, 0.00f }

    };
    public static main_character instance;
    // Attack bool
    public bool canReceiveInput = true;
    public bool inputReceived = false;

    [SerializeField] private LayerMask layerMaskGround;
    [SerializeField] private LayerMask layerMaskEdge; // wall

    Rigidbody2D rigid;
    public Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    public GameObject charater;
    public StaminaBar staminaBar;
    public HealthBar healthBar;
    public heal healAnim;
    public bleeding bleedAnim;

    private float currentMoveValue = 0f;
    private float currentJumpValue = 0f;

    private float currentDistanceRoll = 0f;

    private const float distanceRoll = 7.5f;
    private const float moveSpeed = 10f;
    private const float jumpSpeed = 30f;
    private const float jumpSpeedRatioWhileHoldingEdge = 0.012f;
    private const float moveSpeedRatioWhileHoldingEdge = 0.5f;
    private const float jumpVelocityRatioWhileHoldingEdge = 0.5f;
    private const float timeExecuteJumpWhileHoldingEdge = 0.5f;
    private const float rollSpeed = 20f;
    private const float boostSpeed = 1.25f;
    private const float slowSpeed = 0.75f;
    private const float normalSpeed = 1.0f;

    private const float extraHeight = 0.1f;
    private const float notFallHeight = 3f;
    
    public static bool finishRoll = true;

    private DateTime lastTimeSlide;
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
        lastTimeSlide = DateTime.Now;
        number_flask = max_flask;
        setBoolAnimation("idle");
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.D) /*&& !Input.GetKeyDown(KeyCode.Space)*/)
        {
            spriteRenderer.flipX = false;
            if (finishRoll && meetEdgeAndFall() < 0)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (staminaBar.loseStamina(stamina_amount[stamina_run_boost]))
                    {
                        anim.speed = boostSpeed;
                        currentMoveValue = moveSpeed * boostSpeed;
                        setBoolAnimation(anim_run);
                    }
                }
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                    if (staminaBar.loseStamina(stamina_amount[stamina_run_slowed], true))
                    {
                        anim.speed = slowSpeed;
                        currentMoveValue = moveSpeed * slowSpeed;
                        setBoolAnimation(anim_run);
                    }
                }
                else 
                {
                    if (staminaBar.loseStamina(stamina_amount[stamina_run_normal], true))
                    {
                        anim.speed = normalSpeed;
                        currentMoveValue = moveSpeed * normalSpeed;
                        setBoolAnimation(anim_run);
                    }
                }

                
            }
        }

        else if (Input.GetKey(KeyCode.A)/* && !Input.GetKeyDown(KeyCode.Space)*/)
        {
            spriteRenderer.flipX = true;
            if (finishRoll && meetEdgeAndFall() < 0)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (staminaBar.loseStamina(stamina_amount[stamina_run_boost]))
                    {
                        anim.speed = boostSpeed;
                        currentMoveValue = -moveSpeed * boostSpeed;
                        setBoolAnimation(anim_run);
                    }
                }
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                        if (staminaBar.loseStamina(stamina_amount[stamina_run_slowed], true))
                        {
                        }
                    currentMoveValue = -moveSpeed * slowSpeed;
                    setBoolAnimation(anim_run);
                }
                else
                {
                    if (staminaBar.loseStamina(stamina_amount[stamina_run_normal], true))
                    {
                        anim.speed = normalSpeed;
                        currentMoveValue = -moveSpeed * normalSpeed;
                        setBoolAnimation(anim_run);
                    }
                }
            }
        }
        else
        {
            if (finishRoll && meetEdgeAndFall() < 0)
            {
                if (staminaBar.loseStamina(stamina_amount[stamina_idle], true))
                {
                    currentMoveValue = 0;
                    setBoolAnimation(anim_idle);
                    canReceiveInput = true;
                }
            }
            
        }

        // Jump
        //Debug.Log(isGrounded());
        if ((meetEdgeAndFall() == 1|| isGrounded()) && (Input.GetKeyDown(KeyCode.F)) && staminaBar.loseStamina(stamina_amount[stamina_jump]))
        {
            currentJumpValue = jumpSpeed;
            setBoolAnimation(anim_jump);
        }

        // Roll
        else if (Input.GetKeyDown(KeyCode.Space) && staminaBar.loseStamina(stamina_amount[stamina_roll]))
        {
            StartCoroutine(rollTo());
        }

        // Debug.Log(currentMoveValue);
        // Attack 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log(canReceiveInput);
            if (canReceiveInput && staminaBar.loseStamina(stamina_amount[stamina_attack]))
            {
                currentMoveValue = 0f;
                inputReceived = true;
                canReceiveInput = false;

                // Take stamina
                staminaBar.loseStamina(20);
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
            else if (staminaBar.loseStamina(stamina_amount[stamina_idle_block]))
            {
                setBoolAnimation(anim_idle_block);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            healthBar.takeDamage(20);
            anim.SetTrigger(anim_hurt);
            bleedAnim.playBleedAnimation();
        }

        if (number_flask > 0 && Input.GetKeyDown(KeyCode.Alpha1))
        {
            healAnim.playHealAnimation();
            healthBar.heal(amount_health_heal * healthBar.maxHealth);
            number_flask--;
            
        }

        // Fall
        if (rigid.velocity.y < -0f && !isGrounded(notFallHeight) && staminaBar.loseStamina(stamina_amount[stamina_fall], true))
        {
            Debug.Log("Falling: " + rigid.velocity.y);
            setBoolAnimation(anim_fall);
        }

        int meetEdge = meetEdgeAndFall();
        if ((meetEdge == 0 && Input.GetKey(KeyCode.A)) || (meetEdge == 1 && Input.GetKey(KeyCode.D)))
        {
            if (Input.GetKeyDown(KeyCode.F) && staminaBar.loseStamina(stamina_amount[stamina_jump] * 2/3))
            {
                lastTimeSlide = DateTime.Now;
                setBoolAnimation(anim_jump);
            }
            if (staminaBar.loseStamina(stamina_amount[stamina_wall_slide]))
            {
                double timeDiff = (DateTime.Now - lastTimeSlide).TotalSeconds;
                if (timeDiff >= timeExecuteJumpWhileHoldingEdge)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * jumpVelocityRatioWhileHoldingEdge);
                    setBoolAnimation(anim_wall_slide);
                }
                else
                {
                    Debug.Log("Jumpp while edging");
                    currentJumpValue = jumpSpeed * jumpSpeedRatioWhileHoldingEdge;
                    rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y + currentJumpValue);
                    setBoolAnimation(anim_jump);
                }
            }

        }
        Debug.Log("Speed anim: " + anim.speed + " / Move speed: " + currentMoveValue);
    }

    private bool isGrounded(float height = extraHeight)
    {
       
        RaycastHit2D raycastHit2D = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + height, layerMaskGround);
        return raycastHit2D.collider != null;
    }

    private int isMeetTheEdge()
    {

        RaycastHit2D raycastHitRight2D = Physics2D.Raycast(boxCollider.bounds.center, Vector2.right, boxCollider.bounds.extents.y, layerMaskEdge);
        RaycastHit2D raycastHitLeft2D = Physics2D.Raycast(boxCollider.bounds.center, Vector2.left, boxCollider.bounds.extents.y, layerMaskEdge);
        return raycastHitRight2D.collider != null && !spriteRenderer.flipX ? 1 : raycastHitLeft2D.collider != null && spriteRenderer.flipX ? 0 : -1;
    }

    private int meetEdgeAndFall()
    {
        return isGrounded(1) ? -1 : isMeetTheEdge();
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
            int meetTheEdge = isMeetTheEdge();
            finishRoll = (Math.Abs(transform.position.x - targetX) <= 0.1f) || meetTheEdge >= 0;
            Debug.Log("X: " + transform.position.x   + "v: " + rollSpeed * Time.deltaTime + "Delta roll: " + Math.Abs(transform.position.x - targetX) + " Should stop roll: " + finishRoll);

            yield return null;
        }
        
    }
    public int getCurrentFlaskAmount()
    {
        return number_flask;
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
