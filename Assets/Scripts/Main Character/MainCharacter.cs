using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UIElements;

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
    public const string ANIMATION_COMBO = "combo";

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
    public static string STAMINA_COMBO = "combo";

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
        ANIMATION_DEATH,
        ANIMATION_COMBO
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
        {STAMINA_WALL_SLIDE, 0.00f },
        {STAMINA_COMBO, 40f }
    };

    // Attack settings
    public bool canReceiveInput = true;
    public bool inputReceived = false;

    public Transform attackPoint;
    private const float attackRange = 1.45f;
    private const float damage = 20f;

    [SerializeField] private LayerMask layerMaskGround; // Ground
    [SerializeField] private LayerMask layerMaskEdge; // wall / edge
    [SerializeField] private LayerMask layerMaskCorner; // wall / edge
    [SerializeField] private LayerMask layerMaskEnemy; // Enemy
    [SerializeField] private LayerMask deathLayerMask;

    Rigidbody2D rigid;
    public Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;

    public GameObject charater;
    public StaminaBar staminaBar;
    public HealthBar healthBar;


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
    public bool flipX = false;
    private bool death = false;
    private DateTime lastTimeSlide = DateTime.Now;
    private DateTime lastTimeClickBlock = DateTime.Now;
    private DateTime lastTimeBlock = DateTime.Now;

    private bool canReleaseSkill = false;
    private void Awake()
    {
        instance = this;
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
        if (PauseController.GameIsPaused) {
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
                    AudioManager.Instance.PlaySFXMusic("run", 1.25f);
                }
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                    if (staminaBar.loseStamina(stamina_amount[STAMINA_RUN_SLOWED], true))
                    {
                        anim.speed = slowSpeed;
                        currentMoveValue = moveSpeed * slowSpeed;
                    }
                    AudioManager.Instance.PlaySFXMusic("walk");
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
                {
                    AudioManager.Instance.PlaySFXMusic("run");
                    setBoolAnimation(ANIMATION_RUN);
                }
                else
                {
                    AudioManager.Instance.PlaySFXMusic("slide");
                }
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

        // Combo
        if (canReleaseSkill && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Combo();
        }

        // Attack 
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log(canReceiveInput);
            Attack(damage, stamina_amount[STAMINA_ATTACK]);
        }

        // Roll
        if (
            Input.GetKeyDown(KeyCode.Space) &&
            IsGrounded() &&
            staminaBar.loseStamina(stamina_amount[STAMINA_ROLL])
            )
        {
            AudioManager.Instance.PlaySFXMusic("roll");
            RollEffect.instance.playRollEffectAnimation();
            StartCoroutine(Roll());
        }
        // Jump
        else if (canJump())
        {
            if (!isJumping && (Input.GetKeyDown(KeyCode.F)) && staminaBar.loseStamina(stamina_amount[STAMINA_JUMP]))
            {
                JumpEffect.instance.playJumpEffectAnimation();
                currentJumpValue = jumpSpeed;
                AudioManager.Instance.PlaySFXMusic("jump_start");
                setBoolAnimation(ANIMATION_JUMP);
                isJumping = true;
            }
            else if (isJumping)
            {
                JumpEffect.instance.playLandEffectAnimation();
                isJumping = false;
                AudioManager.Instance.PlaySFXMusic("jump_end");
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

        

        if (number_flask > 0 && Input.GetKeyDown(KeyCode.R))
        {
            heal.instance.playHealAnimation();
            healthBar.heal(amount_health_heal * healthBar.maxHealth);
            number_flask--;
        }

        checkDeathByFall();
        //Debug.Log("Speed anim: " + anim.speed + " / Move speed: " + currentMoveValue);
    }

    // RaycastHit handler
    private bool IsGrounded(float height = extraHeight)
    {
       
        RaycastHit2D raycastHitGround = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down, capsuleCollider.bounds.extents.y + height, layerMaskGround);
        
        return raycastHitGround.collider;
    }

    private bool IsCorner()
    {
        RaycastHit2D raycastHitCorner = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down, capsuleCollider.bounds.extents.y + extraHeight, layerMaskCorner);
        return raycastHitCorner.collider;
    }

    private int IsMeetTheEdge()
    {
        const float dst = 1f;
        if (flipX)
        {
            RaycastHit2D raycastHitLeftEdge = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.left, dst, layerMaskEdge);
            RaycastHit2D raycastHitLeftCorner = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.left, dst * 2, layerMaskCorner);
            return (raycastHitLeftCorner.collider || raycastHitLeftEdge.collider) ? 0: -1;
        }
        else
        {
            RaycastHit2D raycastHitRightEdge = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.right, dst, layerMaskEdge);
            RaycastHit2D raycastHitRightCorner = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.right, dst * 2, layerMaskCorner);
            return (raycastHitRightEdge.collider || raycastHitRightCorner.collider) ? 1 : -1;
        }
    }

    private void checkDeathByFall()
    {

        RaycastHit2D raycastHitDeathLayer = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down, capsuleCollider.bounds.extents.y + extraHeight, deathLayerMask);
        if(raycastHitDeathLayer.collider)
        {
            healthBar.takeDamage(HealthBar.instance.maxHealth);
            death = true;
            DeathBanner.instance.ShowUI();
            DestroyObjectDelayed();
            //GameManager.instance.OnMainCharacterDeath();
            GameManager.instance.Invoke("OnMainCharacterDeath", 2);
        }
    }

    private bool canJump()
    {
        return IsGrounded() | IsCorner();
    }

    private int MeetEdgeAndFall()
    {

        return IsGrounded(extraHeight) ? -1 : IsMeetTheEdge();
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

    void DamageToEnemies(float dmg)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, layerMaskEnemy);
        foreach (Collider2D enemy in hitEnemies)
        {
            //Debug.Log("Hit: " + enemy.name);
            AudioManager.Instance.PlaySFXMusic("hit");
            string name = enemy.name.ToLower();
            if (name == "demon" || name == "nightborne")
            {
                enemy.GetComponent<BossHealth>().takeHit(dmg);
                if (enemy.GetComponent<BossHealth>().IsDeath())
                {
                    souls += enemy.GetComponent<BossGeneral>().getSouls(name);
                    SoulAmount.instance.UpdateSouls(souls);
                }
            }
            else 
            {
                try
                {
                    enemy.GetComponent<enemy_damage>().TakeDamage(dmg);
                    if (enemy.GetComponent<enemy_damage>().isDeath())
                    {
                        souls += enemy.GetComponent<enemy_damage>().getSouls();
                        SoulAmount.instance.UpdateSouls(souls);
                    }
                } 
                catch {
                    Debug.Log("Some error happened in MainCharacter.cs: DamageToEnemies");
                }
            }
        }
    }
    void Attack(float damage, float stamina_cost)
    {
        if (canReceiveInput && staminaBar.loseStamina(stamina_cost))
        {
            currentMoveValue = 0f;
            inputReceived = true;
            canReceiveInput = false;
            DamageToEnemies(damage);
        }
    }

    void Combo()
    {
        if (staminaBar.loseStamina(stamina_amount[STAMINA_COMBO]))
        {
            // Dash
            DashEffect.instance.playDashEffectAnimation();
            float positionX = gameObject.transform.position.x;
            float enemyX = flipX ? -5 : 5;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            foreach(GameObject enemy in enemies)
            {
                if(Mathf.Abs(enemyX) > Mathf.Abs(enemy.transform.position.x - positionX))
                {
                    enemyX = enemy.transform.position.x - positionX;
                }
            }
            if (enemyX < 0)
            {
                enemyX += 1f;
                transform.eulerAngles = new Vector3(0, 180, 0);
                flipX = true;
            }
            else
            {
                enemyX -= 1f;
                transform.eulerAngles = new Vector3(0, 0, 0);
                flipX = false;
            }
            transform.position = new Vector2(transform.position.x + enemyX, transform.position.y);
            // Combo
            ComboAttack1.instance.playAttack1EffectAnimation();
            ComboAttack2.instance.playAttack2EffectAnimation();
            AudioManager.Instance.PlaySFXMusic("combo");
            setTriggerAnimation(ANIMATION_COMBO);
            DamageToEnemies(damage * 3);
        }
    }

    public void EnableSkill()
    {
        canReleaseSkill = true;
    }
    public void PlayAttackSound(int i)
    {
        AudioManager.Instance.PlaySFXMusic("attack" + i);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public bool Death()
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
        else if (isBlocking && staminaBar.loseStamina(stamina_amount[STAMINA_NORMAL_BLOCK]))
        {
            if ((lastTimeBlock - lastTimeClickBlock).TotalSeconds >= 0.5f){

                healthBar.takeDamage(dmg / 3);
                // Death
                if (Death())
                {
                    death = true;
                    DeathBanner.instance.ShowUI();
                    DestroyObjectDelayed();
                    GameManager.instance.Invoke("OnMainCharacterDeath",2);
                }
                else
                {
                    setTriggerAnimation(ANIMATION_NORMAL_BLOCK);
                    float deltaX = flipX ? 1 : -1;
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        new Vector3(transform.position.x + deltaX, transform.position.y, transform.position.z),
                        moveSpeed * Time.deltaTime);
                    AudioManager.Instance.PlaySFXMusic("block");
                }
            }
            else
            {
                setTriggerAnimation(ANIMATION_PERFECT_BLOCK);
                healthBar.heal(dmg);
                AudioManager.Instance.PlaySFXMusic("perfect_block");
            }
        }
        else
        {

            healthBar.takeDamage(dmg);
            // Death
            if (healthBar.death())
            {
                death = true;
                AudioManager.Instance.PlaySFXMusic("death");
                DeathBanner.instance.ShowUI();
                DestroyObjectDelayed();
                //GameManager.instance.OnMainCharacterDeath();
                GameManager.instance.Invoke("OnMainCharacterDeath", 2);
            }
            else
            {
                setTriggerAnimation(ANIMATION_HURT);
                AudioManager.Instance.PlaySFXMusic("hurt");
                bleeding.instance.playBleedAnimation();
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
