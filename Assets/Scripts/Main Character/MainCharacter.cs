using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor.Search;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using static Cinemachine.CinemachineTargetGroup;

public class main_character : MonoBehaviour
{
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
    public const string ANIMATION_NORMAL_BLOCK = "normal_block";
    public const string ANIMATION_PERFECT_BLOCK = "perfect_block";
    public const string ANIMATION_ATTACK_ONE = "attack1";
    public const string ANIMATION_ATTACK_TWO = "attack2";
    public const string ANIMATION_ATTACK_THREE = "attack3";
    public const string ANIMATION_ATTACK_COMBO = "combo1";
    public const string ANIMATION_HURT = "hurt";
    public const string ANIMATION_WALL_SLIDE = "wall_slide";
    public const string ANIMATION_DEATH = "death";

    // List stamina cost
    public static string STAMINA_RUN_BOOST = "run--boost";
    public static string STAMINA_RUN_SLOWED = "run--slowed";
    public static string STAMINA_RUN = "run";
    public static string STAMINA_IDLE = "idle";
    public static string STAMINA_ROLL = "roll";
    public static string STAMINA_JUMP = "jump";
    public static string STAMINA_FALL = "fall";
    public static string STAMINA_IDLE_BLOCK = "block";
    public static string STAMINA_NORMAL_BLOCK = "normal_block";
    public static string STAMINA_ATTACK = "attack";
    public static string STAMINA_WALL_SLIDE = "wall_slide";


    List<string> list_bool_anims = new List<string>() {
        ANIMATION_RUN,
        ANIMATION_IDLE,
        ANIMATION_ROLL,
        ANIMATION_JUMP,
        ANIMATION_IDLE_BLOCK,
        ANIMATION_FALL,
        ANIMATION_WALL_SLIDE 
    };

    List<string> list_trigger_anims = new List<string>() { 
        ANIMATION_ATTACK_ONE, 
        ANIMATION_ATTACK_TWO, 
        ANIMATION_ATTACK_THREE, 
        ANIMATION_ATTACK_COMBO,
        ANIMATION_JUMP,
        ANIMATION_ROLL,
        ANIMATION_HURT,
        ANIMATION_NORMAL_BLOCK,
        ANIMATION_PERFECT_BLOCK,
        ANIMATION_DEATH
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
        {STAMINA_NORMAL_BLOCK, 15f },
        {STAMINA_WALL_SLIDE, 0.00f }
    };

    public static main_character instance;
    // Attack bool
    public bool canReceiveInput = true;
    public bool inputReceived = false;

    [SerializeField] private LayerMask layerMaskGround; // Ground
    [SerializeField] private LayerMask layerMaskEdge; // wall / edge
    [SerializeField] private LayerMask layerMaskEnemy;


    Rigidbody2D rigid;
    public Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;

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
    private const  float jumpVelocityRatioWhileHoldingEdge = 2.75f;
    private const float resistVelocityRatioWhileHoldingEdge = 0.5f;
    private const float timeExecuteJumpWhileHoldingEdge = 0.5f;
    private const float rollSpeed = 20f;
    private const float boostSpeed = 1.25f;
    private const float slowSpeed = 0.75f;
    private const float normalSpeed = 1.0f;

    private const float extraHeight = 0.01f;
    private const float notFallHeight = 3f;
    
    public static bool finishRoll = true;
    private bool isBlocking = false;
    private bool flipX = false;
    private DateTime lastTimeSlide = DateTime.Now;
    private DateTime lastTimeClickBlock = DateTime.Now;
    private DateTime lastTimeBlock = DateTime.Now;


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
        capsuleCollider = GetComponent <CapsuleCollider2D>();
        number_flask = max_flask;
    }


    // Update is called once per frame
    void Update()
    {
        if(ControlOptions.Instance.CheckOpen()){
            return;
        }
        Debug.Log("Fps: " + 1.0f / Time.deltaTime);

        int meetEdge = meetEdgeAndFall();
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            
            if (finishRoll)
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
                if (meetEdge < 0)
                setBoolAnimation(ANIMATION_RUN);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                flipX = false;

            }
            else
            {
                currentMoveValue *= -1;
                transform.eulerAngles = new Vector3(0, 180, 0);
                flipX = true;
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
            }        
        }

        // Attack 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log(canReceiveInput);
            if (canReceiveInput && staminaBar.loseStamina(stamina_amount[STAMINA_ATTACK]))
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
                setTriggerAnimation(ANIMATION_ATTACK_COMBO);
            }
            else if (staminaBar.loseStamina(stamina_amount[STAMINA_IDLE_BLOCK]))
            {
                setBoolAnimation(ANIMATION_IDLE_BLOCK);
            }
            isBlocking = true;
            lastTimeBlock = DateTime.Now;
        }
        else
        {
            lastTimeClickBlock = DateTime.Now;
            isBlocking = false;
        }


        // Fall
        if ( meetEdge < 0 && rigid.velocity.y < -0f && !isGrounded(notFallHeight) && staminaBar.loseStamina(stamina_amount[STAMINA_FALL], true))
        {
            setBoolAnimation(ANIMATION_FALL);
        }
        // Holding Edge
        if (
            (meetEdge == 0 && Input.GetKey(KeyCode.A)) ||
            (meetEdge == 1 && Input.GetKey(KeyCode.D))
            )
        {
            if (Input.GetKeyDown(KeyCode.F) && staminaBar.loseStamina(stamina_amount[STAMINA_JUMP] * 2 / 3))
            {
                lastTimeSlide = DateTime.Now;
            }
            if (staminaBar.loseStamina(stamina_amount[STAMINA_WALL_SLIDE]))
            {
                double timeDiff = (DateTime.Now - lastTimeSlide).TotalSeconds;
                if (timeDiff >= timeExecuteJumpWhileHoldingEdge)
                {
                    currentJumpValue = jumpSpeed * resistVelocityRatioWhileHoldingEdge * Time.deltaTime;
                    rigid.velocity = new Vector2(rigid.velocity.x + currentMoveValue * Time.deltaTime, currentJumpValue);
                    setBoolAnimation(ANIMATION_WALL_SLIDE);
                }
                else
                {
                    //Debug.Log("Jumpp while edging");
                    currentJumpValue = jumpSpeed * jumpVelocityRatioWhileHoldingEdge * Time.deltaTime;
                    rigid.velocity = new Vector2(rigid.velocity.x + currentMoveValue * Time.deltaTime, Math.Abs(rigid.velocity.y) + currentJumpValue);
                    setBoolAnimation(ANIMATION_JUMP);
                }
            }
        }
        // Jump
        if (
            (isGrounded()) && 
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

        if (number_flask > 0 && Input.GetKeyDown(KeyCode.R))
        {
            healAnim.playHealAnimation();
            healthBar.heal(amount_health_heal * healthBar.maxHealth);
            number_flask--;
        }

        Debug.Log("Speed anim: " + anim.speed + " / Move speed: " + currentMoveValue);
    }

    // RaycastHit handler
    private bool isGrounded(float height = extraHeight)
    {
       
        RaycastHit2D raycastHit2D = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down, capsuleCollider.bounds.extents.y + height, layerMaskGround);
        return raycastHit2D.collider != null;
    }

    private int isMeetTheEdge()
    {
        RaycastHit2D raycastHit2D = flipX ?
            Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.left, 1.22f, layerMaskEdge) :
            Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.right, 1.22f, layerMaskEdge);

        return !raycastHit2D.collider ? -1 : !flipX ? 1 : 0;
    }

    private int meetEdgeAndFall()
    {
        return isGrounded(extraHeight)? -1 : isMeetTheEdge();
    }

    private IEnumerator rollTo()
    {
        currentDistanceRoll = !flipX ? distanceRoll : -distanceRoll;
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
        if (!isBlocking)
        {

            healthBar.takeDamage(dmg); 
            // Death
            if (healthBar.death())
            {
                DestroyObjectDelayed();
            }
            else
            {
                setTriggerAnimation(ANIMATION_HURT);
                bleedAnim.playBleedAnimation();
            }

        }
        else if (staminaBar.loseStamina(stamina_amount[STAMINA_NORMAL_BLOCK]))
        {
            if ((lastTimeBlock - lastTimeClickBlock).TotalSeconds >= 0.5f){

                healthBar.takeDamage(dmg / 3);
                // Death
                if (healthBar.death())
                {
                    DestroyObjectDelayed();
                }
                else
                {
                    setTriggerAnimation(ANIMATION_NORMAL_BLOCK);
                    float deltaX = flipX ? 1 : -1;
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        new Vector3(transform.position.x + deltaX, transform.position.y, transform.position.z),
                        moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                setTriggerAnimation(ANIMATION_PERFECT_BLOCK);
                healthBar.heal(dmg / 3);
            }
        }
    }

    public void inputManager()
    {
        canReceiveInput = !canReceiveInput;
    }

    // Animation handler
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

    void DestroyObjectDelayed()
    {
        setTriggerAnimation(ANIMATION_DEATH);
        Destroy(gameObject, 2.5f);
    }

    private void FixedUpdate()
    {
        //Debug.Log("Finish roll: " + finishRoll);
        if (finishRoll)
        {
            //Debug.Log("Jump: " + currentJumpValue);
            rigid.velocity = new Vector2(currentMoveValue, rigid.velocity.y + currentJumpValue);
        }

        currentJumpValue = 0;
    }
}
