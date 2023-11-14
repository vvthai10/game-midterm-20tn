using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.EventSystems.EventTrigger;

public class main_character : MonoBehaviour
{

    public static main_character instance;

    public int number_flask;
    public int souls;
    public const int max_flask = 5;
    private float amount_health_heal = 0.3f;

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

    // Attack settings
    public bool canReceiveInput = true;
    public bool inputReceived = false;

    public Transform attackPoint;
    private const float attackRange = 1.45f;
    private const float damage = 20f;

    [SerializeField] private LayerMask layerMaskGround; // Ground
    [SerializeField] private LayerMask layerMaskEdge; // wall / edge
    [SerializeField] private LayerMask layerMaskEnemy; // Enemy


    Rigidbody2D rigid;
    public Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;
    AudioManager audioManager;

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
    private const  float jumpVelocityRatioWhileHoldingEdge = 2.5f;
    private const float resistVelocityRatioWhileHoldingEdge = 0.5f;
    private const float timeExecuteJumpWhileHoldingEdge = 0.5f;
    private const float rollSpeed = 20f;
    private const float boostSpeed = 1.25f;
    private const float slowSpeed = 0.75f;
    private const float normalSpeed = 1.0f;

    private const float extraHeight = 0.01f;
    private const float notFallHeight = 3f;
    
    public static bool finishRoll = true;
    private bool isJumping = false;
    private bool isBlocking = false;
    private bool flipX = false;
    private bool death = false;
    private DateTime lastTimeSlide = DateTime.Now;
    private DateTime lastTimeClickBlock = DateTime.Now;
    private DateTime lastTimeBlock = DateTime.Now;


    private void Awake()
    {
        instance = this;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
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
        //Debug.Log("Fps: " + 1.0f / Time.deltaTime);

        if (death)
        {
            return;
        }

        int meetEdge = MeetEdgeAndFall();
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
                    audioManager.PlaySFXMusic("run", 1.25f);
                }
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                    if (staminaBar.loseStamina(stamina_amount[STAMINA_RUN_SLOWED], true))
                    {
                        anim.speed = slowSpeed;
                        currentMoveValue = moveSpeed * slowSpeed;
                    }
                    audioManager.PlaySFXMusic("walk");
                }
                else 
                {
                    if (staminaBar.loseStamina(stamina_amount[STAMINA_RUN], true))
                    {
                        anim.speed = normalSpeed;
                        currentMoveValue = moveSpeed * normalSpeed;
                    }
                    audioManager.PlaySFXMusic("run");
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
            if (finishRoll && MeetEdgeAndFall() < 0)
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
            Attack();
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
        if ( meetEdge < 0 && rigid.velocity.y < -0f && !IsGrounded(notFallHeight) && staminaBar.loseStamina(stamina_amount[STAMINA_FALL], true))
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
            (IsGrounded()) && 
            (Input.GetKeyDown(KeyCode.F)) && 
            staminaBar.loseStamina(stamina_amount[STAMINA_JUMP])
            )
        {
            currentJumpValue = jumpSpeed;
            audioManager.PlaySFXMusic("jump_start");
            setBoolAnimation(ANIMATION_JUMP);
            isJumping = true;
        }
        else if (isJumping && IsGrounded())
        {
            isJumping = false;
            audioManager.PlaySFXMusic("jump_end");
        }

        // Roll
        else if (
            Input.GetKeyDown(KeyCode.Space) && 
            staminaBar.loseStamina(stamina_amount[STAMINA_ROLL])
            )
        {
            audioManager.PlaySFXMusic("roll");
            StartCoroutine(Roll());
        }

        if (number_flask > 0 && Input.GetKeyDown(KeyCode.R))
        {
            healAnim.playHealAnimation();
            healthBar.heal(amount_health_heal * healthBar.maxHealth);
            number_flask--;
        }

        //Debug.Log("Speed anim: " + anim.speed + " / Move speed: " + currentMoveValue);
    }

    // RaycastHit handler
    private bool IsGrounded(float height = extraHeight)
    {
       
        RaycastHit2D raycastHit2D = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down, capsuleCollider.bounds.extents.y + height, layerMaskGround);
        return raycastHit2D.collider != null;
    }

    private int IsMeetTheEdge()
    {
        RaycastHit2D raycastHit2D = flipX ?
            Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.left, 1.22f, layerMaskEdge) :
            Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.right, 1.22f, layerMaskEdge);

        return !raycastHit2D.collider ? -1 : !flipX ? 1 : 0;
    }

    private int MeetEdgeAndFall()
    {
        return IsGrounded(extraHeight)? -1 : IsMeetTheEdge();
    }

    // Roll
    private IEnumerator Roll()
    {
        currentDistanceRoll = !flipX ? distanceRoll : -distanceRoll;
        float targetX = transform.position.x + currentDistanceRoll;
        setBoolAnimation(ANIMATION_ROLL);
        finishRoll = false;
        while (!finishRoll)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), rollSpeed * Time.deltaTime);
            int meetTheEdge = IsMeetTheEdge();
            finishRoll = (Math.Abs(transform.position.x - targetX) <= 0.1f) || meetTheEdge >= 0;
            //Debug.Log("X: " + transform.position.x   + "v: " + rollSpeed * Time.deltaTime + "Delta roll: " + Math.Abs(transform.position.x - targetX) + " Should stop roll: " + finishRoll);

            yield return null;
        }
    }
    public int GetCurrentFlaskAmount()
    {
        return number_flask;
    }

    void Attack()
    {
        if (canReceiveInput && staminaBar.loseStamina(stamina_amount[STAMINA_ATTACK]))
        {
            currentMoveValue = 0f;
            inputReceived = true;
            canReceiveInput = false;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, layerMaskEnemy);
            foreach(Collider2D enemy in hitEnemies)
            {
                Debug.Log("Hit: " + enemy.name);
                audioManager.PlaySFXMusic("hit");
                string name = enemy.name.ToLower();
                if (name == "demon" || name == "nightborne")
                {
                    enemy.GetComponent<BossHealth>().takeHit(damage);
                    if (enemy.GetComponent<BossHealth>().IsDeath()) {
                        souls += enemy.GetComponent<BossGeneral>().getSouls(name);
                        SoulAmount.instance.UpdateSouls(souls);
                    }
                }
                else
                {
                    enemy.GetComponent<enemy_damage>().TakeDamage(damage);
                    if (enemy.GetComponent<enemy_damage>().isDeath())
                    {
                        souls += enemy.GetComponent<enemy_damage>().getSouls();
                        SoulAmount.instance.UpdateSouls(souls);
                    }
                }
            }
        }
    }
    public void PlayAttackSound(int i)
    {
        audioManager.PlaySFXMusic("attack" + i);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private bool Death()
    {
        death = healthBar.death();
        return death;
    }

    public void TakeDameage(float dmg)
    {
        if (death)
        {
            return;
        }
        if (!isBlocking)
        {

            healthBar.takeDamage(dmg); 
            // Death
            if (healthBar.death())
            {
                death = true;
                audioManager.PlaySFXMusic("death");
                DeathBanner.instance.ShowUI();
                DestroyObjectDelayed();
            }
            else
            {
                setTriggerAnimation(ANIMATION_HURT);
                audioManager.PlaySFXMusic("hurt");
                bleedAnim.playBleedAnimation();
            }

        }
        else if (staminaBar.loseStamina(stamina_amount[STAMINA_NORMAL_BLOCK]))
        {
            if ((lastTimeBlock - lastTimeClickBlock).TotalSeconds >= 0.5f){

                healthBar.takeDamage(dmg / 3);
                // Death
                if (Death())
                {
                    death = true;
                    audioManager.PlaySFXMusic("death");
                    DeathBanner.instance.ShowUI();
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
                    audioManager.PlaySFXMusic("block");
                }
            }
            else
            {
                setTriggerAnimation(ANIMATION_PERFECT_BLOCK);
                healthBar.heal(dmg);
                audioManager.PlaySFXMusic("perfect_block");
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

    public void IncreaseFlask(int numberFlash)
    {
        number_flask += numberFlash;
    }

    public void BoughtItem(float money)
    {
        souls -=(int)money;
        SoulAmount.instance.UpdateSouls(souls);

    }
    private void FixedUpdate()
    {
        if (death)
            return;

        //Debug.Log("Finish roll: " + finishRoll);
        if (finishRoll)
        {
            //Debug.Log("Jump: " + currentJumpValue);
            rigid.velocity = new Vector2(currentMoveValue, rigid.velocity.y + currentJumpValue);
        }

        currentJumpValue = 0;
    }
}
