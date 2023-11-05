using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor.Search;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class main_character : MonoBehaviour
{
    public const int FPS = 60;
    public int number_flask;
    public const int max_flask = 5;
    private float amount_health_heal = 0.2f;

    // List animation name
    public const string ANIMATION_RUN = "run";
    public const string ANIMATION_IDLE = "idle";
    public const string ANIMATION_ROLL = "roll";
    public const string ANIMATION_JUMP = "jump";
    public const string ANIMATION_FALL = "fall";
    public const string ANIMATION_IDLE_BLOCK = "block";
    public const string ANIMATION_ATTACK_ONE = "attack1";
    public const string ANIMATION_ATTACK_TWO = "attack2";
    public const string ANIMATION_ATTACK_THREE = "attack3";
    public const string ANIMATION_ATTACK_COMBO = "combo1";
    public const string ANIMATION_HURT = "hurt";
    public const string ANIMATION_WALL_SLIDE = "wall_slide";

    // List stamina cost
    public static string STAMINA_RUN_BOOST = "run--boost";
    public static string STAMINA_RUN_SLOWED = "run--slowed";
    public static string STAMINA_RUN = "run";
    public static string STAMINA_IDLE = "idle";
    public static string STAMINA_ROLL = "roll";
    public static string STAMINA_JUMP = "jump";
    public static string STAMINA_FALL = "fall";
    public static string STAMINA_IDLE_BLOCK = "block";
    public static string STAMINA_ATTACK = "attack";
    public static string STAMINA_WALL_SLIDE = "wall_slide";

    List<string> list_bool_anims = new List<string>() {
        ANIMATION_RUN,
        ANIMATION_IDLE,
        ANIMATION_ROLL,
        ANIMATION_JUMP,
        ANIMATION_IDLE_BLOCK,
        ANIMATION_FALL,
        ANIMATION_WALL_SLIDE };

    List<string> list_trigger_anims = new List<string>() { 
        ANIMATION_ATTACK_ONE, 
        ANIMATION_ATTACK_TWO, 
        ANIMATION_ATTACK_THREE, 
        ANIMATION_ATTACK_COMBO,
        ANIMATION_JUMP,
        ANIMATION_ROLL,
        ANIMATION_HURT
    };

    Dictionary<string, float> stamina_amount = new Dictionary<string, float>() {
        {STAMINA_RUN_BOOST, 0.1f },
        {STAMINA_RUN, 0f},
        {STAMINA_RUN_SLOWED, -0.01f},
        {STAMINA_ROLL, 20f },
        {STAMINA_JUMP, 15f},
        {STAMINA_ATTACK, 25f },
        {STAMINA_IDLE, -0.025f },
        {STAMINA_FALL, 0f },
        {STAMINA_IDLE_BLOCK, 0.1f },
        {STAMINA_WALL_SLIDE, 0.00f }

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
    private const float jumpVelocityRatioWhileHoldingEdge = 0.0425f;
    private const float resistVelocityRatioWhileHoldingEdge = 0.075f;
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
        Application.targetFrameRate = FPS;
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
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            
            if (finishRoll && meetEdgeAndFall() < 0)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (staminaBar.loseStamina(stamina_amount[STAMINA_RUN_BOOST]))
                    {
                        anim.speed = boostSpeed;
                        currentMoveValue = moveSpeed * boostSpeed;
                    }
                }
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                    if (staminaBar.loseStamina(stamina_amount[STAMINA_RUN_SLOWED], true))
                    {
                        anim.speed = slowSpeed;
                        currentMoveValue = moveSpeed * slowSpeed;
                    }
                }
                else 
                {
                    if (staminaBar.loseStamina(stamina_amount[STAMINA_RUN], true))
                    {
                        anim.speed = normalSpeed;
                        currentMoveValue = moveSpeed * normalSpeed;
                    }
                }
                setBoolAnimation(ANIMATION_RUN);
            }

            if (Input.GetKey(KeyCode.D))
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
                currentMoveValue *= -1;
            }
        }

        else
        {
            if (finishRoll && meetEdgeAndFall() < 0)
            {
                if (staminaBar.loseStamina(stamina_amount[STAMINA_IDLE], true))
                {
                    currentMoveValue = 0;
                    setBoolAnimation(ANIMATION_IDLE);
                    canReceiveInput = true;
                }
            }        }

        // Jump
        if (
            (meetEdgeAndFall() == 1|| isGrounded()) && 
            (Input.GetKeyDown(KeyCode.F)) && 
            staminaBar.loseStamina(stamina_amount[STAMINA_JUMP])
            )
        {
            currentJumpValue = jumpSpeed;
            setBoolAnimation(ANIMATION_JUMP);
        }

        // Roll
        else if (
            Input.GetKeyDown(KeyCode.Space) && 
            staminaBar.loseStamina(stamina_amount[STAMINA_ROLL])
            )
        {
            StartCoroutine(rollTo());
        }

        // Debug.Log(currentMoveValue);
        // Attack 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log(canReceiveInput);
            if (canReceiveInput && staminaBar.loseStamina(stamina_amount[STAMINA_ATTACK]))
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
                setTriggerAnimation(ANIMATION_ATTACK_COMBO);
            }
            else if (staminaBar.loseStamina(stamina_amount[STAMINA_IDLE_BLOCK]))
            {
                setBoolAnimation(ANIMATION_IDLE_BLOCK);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            healthBar.takeDamage(20);
            setTriggerAnimation(ANIMATION_HURT);
            bleedAnim.playBleedAnimation();
        }

        if (number_flask > 0 && Input.GetKeyDown(KeyCode.R))
        {
            healAnim.playHealAnimation();
            healthBar.heal(amount_health_heal * healthBar.maxHealth);
            number_flask--;
        }

        // Fall
        if (rigid.velocity.y < -0f && !isGrounded(notFallHeight) && staminaBar.loseStamina(stamina_amount[STAMINA_FALL], true))
        {
            setBoolAnimation(ANIMATION_FALL);
        }

        int meetEdge = meetEdgeAndFall();
        if ((meetEdge == 0 && Input.GetKey(KeyCode.A)) || (meetEdge == 1 && Input.GetKey(KeyCode.D)))
        {
            if (Input.GetKeyDown(KeyCode.F) && staminaBar.loseStamina(stamina_amount[STAMINA_JUMP] * 2/3))
            {
                lastTimeSlide = DateTime.Now;
                setBoolAnimation(ANIMATION_JUMP);
            }
            if (staminaBar.loseStamina(stamina_amount[STAMINA_WALL_SLIDE]))
            {
                double timeDiff = (DateTime.Now - lastTimeSlide).TotalSeconds;
                if (timeDiff >= timeExecuteJumpWhileHoldingEdge)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * resistVelocityRatioWhileHoldingEdge);
                    setBoolAnimation(ANIMATION_WALL_SLIDE);
                }
                else
                {
                    //Debug.Log("Jumpp while edging");
                    currentJumpValue = jumpSpeed * jumpVelocityRatioWhileHoldingEdge;
                    rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y + currentJumpValue);
                    setBoolAnimation(ANIMATION_JUMP);
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
        return isGrounded(1.5f)? -1 : isMeetTheEdge();
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
        setBoolAnimation(ANIMATION_ROLL);
        finishRoll = false;
        while (!finishRoll)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), rollSpeed * Time.deltaTime);
            int meetTheEdge = isMeetTheEdge();
            finishRoll = (Math.Abs(transform.position.x - targetX) <= 0.1f) || meetTheEdge >= 0;
            //Debug.Log("X: " + transform.position.x   + "v: " + rollSpeed * Time.deltaTime + "Delta roll: " + Math.Abs(transform.position.x - targetX) + " Should stop roll: " + finishRoll);

            yield return null;
        }
    }
    public int getCurrentFlaskAmount()
    {
        return number_flask;
    }

    public void takeDameage(float dmg)
    {
        healthBar.takeDamage(dmg);
        setTriggerAnimation(ANIMATION_HURT);
        bleedAnim.playBleedAnimation();
    }
    public void inputManager()
    {
        canReceiveInput = !canReceiveInput;
    }

    private void setAllBoolAnimationOff()
    {
        for (int i = 0; i < list_bool_anims.Count; i++)
        {
            anim.SetBool(list_bool_anims[i], false);
        }
    }

    void setBoolAnimation(string animName)
    {
        setAllBoolAnimationOff();
        anim.SetBool(animName, true);
    }

    public void setTriggerAnimation(string animName)
    {
        setAllBoolAnimationOff();
        anim.SetTrigger(animName);
    }
    private void FixedUpdate()
    {
        //Debug.Log("Finish roll: " + finishRoll);
        if (finishRoll)
        {
            rigid.velocity = new Vector2(currentMoveValue, rigid.velocity.y + currentJumpValue);
        }
        
        currentJumpValue = 0;
    }
}
