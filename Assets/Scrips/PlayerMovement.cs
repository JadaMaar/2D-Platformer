using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    public GameObject Fireball;
    public GameObject LavaPillar;
    public GameObject Earthwall;
    public GameObject Laser;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private CapsuleCollider2D bodyColider;
    private BoxCollider2D feetCollider;
    private LineRenderer lr;
    private SpriteRenderer sr;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    //jump stuff
    bool doubleJump = true;
    private float startGravity;

    const float jumpAfterLedgeDelayStart = .1f;
    private float jumpAfterLedgeDelay = jumpAfterLedgeDelayStart;

    bool grounded;

    //Roll cooldown
    const float RollDelayStart = .5f;
    float RollDelay = RollDelayStart;
    private float RollSpeedMultiplier = 1f;

    //Attack cooldowns
    const float Magic1CDstart = .5f; //Fireball
    float Magic1CD = Magic1CDstart;
    const float Magic2CDstart = 2f; //Earthwall
    float Magic2CD = Magic2CDstart;
    private bool isAlive = true;

    //Attacks unlocked
    public bool FireballUnlocked { get; set; }
    public bool LightningUnlocked { get; set; }
    public bool LavaUnlocked { get; set; }

    private bool knockback = false; //loose controll if true

    const int MaxHP = 5;
    private int HP = MaxHP;

    private static PlayerMovement instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        bodyColider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        lr = GetComponent<LineRenderer>();
        sr = GetComponent<SpriteRenderer>();
        startGravity = rb.gravityScale;

        FireballUnlocked = true;
        LightningUnlocked = true;
    }


    void Update()
    {
        if (!isAlive) { return; }
        if (myAnimator.GetBool("isRolling") || knockback) { return;  }

        Run();
        FlipSprite();
        ClimbLadder();        

        grounded = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (grounded)
        {
            jumpAfterLedgeDelay = jumpAfterLedgeDelayStart;
            doubleJump = true;
            RollSpeedMultiplier = 1f;
        }
        else
        {
            jumpAfterLedgeDelay -= Time.deltaTime;
        }
        RollDelay -= Time.deltaTime;
        Magic1CD -= Time.deltaTime;
        Magic2CD -= Time.deltaTime;

        if (bodyColider.IsTouchingLayers(LayerMask.GetMask("Truck-kun"))) { Destroy(this.gameObject);  }
    }


    //Movement
    void OnMove(InputValue value)
     {
         moveInput = value.Get<Vector2>();
         //Debug.Log(moveInput);
     }

    void OnJump(InputValue value)
    {
        bool grounded = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (!myAnimator.GetBool("isRolling") && value.isPressed && (grounded || doubleJump || jumpAfterLedgeDelay > 0))
        {
            if (doubleJump && !grounded && jumpAfterLedgeDelay <= 0)
            {
                doubleJump = false;
                /*RaycastHit2D hit = Physics2D.Raycast(transform.position, rb.velocity, 4);
                if(hit == false)
                {
                    Vector3 v = new Vector3(rb.velocity.x, rb.velocity.y, 0).normalized * 4;
                    transform.position += v;
                }
                else
                {
                    transform.position = hit.point - rb.velocity.normalized;
                }*/
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speed * RollSpeedMultiplier, rb.velocity.y);
        rb.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }

    }

    void ClimbLadder()
    {
        bool ladder = feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        bool playerHasVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", ladder && playerHasVerticalSpeed);
        if (!ladder)
        {
            rb.gravityScale = startGravity;
            return;
        }

        
        Vector2 climbVelocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
        rb.velocity = climbVelocity;
        rb.gravityScale = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }

        if (myAnimator.GetBool("isRolling"))
        {
            myAnimator.SetBool("isRolling", false);
            RollSpeedMultiplier = 1f;
            Knockback(new Vector2(-transform.localScale.x, 1.5f), 10);
        }
    }

    private void Die()
    {
        isAlive = false;
    }


    
    //Abilities
    private void OnDodge(InputValue value)  //Press Shift
    {
        Surfer();
    }

    private void Surfer()
    {
        if (!myAnimator.GetBool("isRolling") && !myAnimator.GetBool("isClimbing") && RollDelay <= 0)
        {
            RollSpeedMultiplier = 2f;
            myAnimator.SetBool("isRolling", true);
            rb.velocity = new Vector2(transform.localScale.x * speed * RollSpeedMultiplier, 0f);
            rb.gravityScale = 0;
            Debug.Log(rb.velocity);
            StartCoroutine(Roll());
        }
        else if (myAnimator.GetBool("isRolling"))
        {
            StopCoroutine(Roll());
            myAnimator.SetBool("isRolling", false);
            RollDelay = RollDelayStart;
            rb.gravityScale = startGravity;
        }
    }

    IEnumerator Roll()
    {
        yield return new WaitForSeconds(.417f);
        Debug.Log("rollend");
        myAnimator.SetBool("isRolling", false);
        RollDelay = RollDelayStart;
        rb.gravityScale = startGravity;
    }

    private void OnMagic(InputValue value)  //Press Q
    {
        //FireballCast();
        LaserShot();
    }

    private void FireballCast()     //Fireball
    {
        if (!FireballUnlocked) { return; }

        if (!myAnimator.GetBool("isRolling") && !myAnimator.GetBool("isClimbing") && Magic1CD <= 0)
        {
            float direction = transform.localScale.x;
            Instantiate(Fireball, transform.position + new Vector3(.5f, 0, 0) * direction, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0) * direction;
            Magic1CD = Magic1CDstart;
        }
    }

    private void LaserShot()    //Laser (Fire + Lightning)
    {
        if (!myAnimator.GetBool("isRolling") && !myAnimator.GetBool("isClimbing") && Magic1CD <= 0)
        {
            myAnimator.Play("Paul Laser Gun", -1, 0f);
            float direction = transform.localScale.x;
            Instantiate(Laser, transform.position + new Vector3 (1, 0, 0) * direction, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(15f, 0) * direction;
            Magic1CD = Magic1CDstart;
        }
    }

    private void OnMagic1(InputValue value) //Press F
    {
        EarthwallCast();
    }

    private void EarthwallCast()    //Earthwall
    {
        if (grounded && !myAnimator.GetBool("isRolling") && !myAnimator.GetBool("isClimbing") && Magic2CD <= 0)
        {
            float direction = transform.localScale.x;
            Instantiate(Earthwall, transform.position + new Vector3(1.2f * direction, .55f, 0), Quaternion.identity).transform.localScale = new Vector2(direction, 1);
            Magic2CD = Magic2CDstart;
        }
    }

    

    //Enemy interaction
    public void TakeDamage(int dmg)
    {
        Debug.Log("uff");
        HP -= dmg;
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }

        if(HP <= 0.5f * MaxHP)
        {
            sr.color = Color.red;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    public void Knockback(Vector2 direction, float strength)
    {
        rb.AddForce(direction.normalized * strength, ForceMode2D.Impulse);
        knockback = true;
        StartCoroutine(LooseControll());
    }

    IEnumerator LooseControll()
    {
        yield return new WaitForSeconds(.1f);
        knockback = false;
    }


    //Cooldowns
    public float GetMagic1CD()
    {
        return Magic1CD;
    }

    public float GetMagic1CDstart()
    {
        return Magic1CDstart;
    }

    public float GetMagic2CD()
    {
        return Magic2CD;
    }

    public float GetMagic2CDstart()
    {
        return Magic2CDstart;
    }

    public float GetDodgeCD()
    {
        return RollDelay;
    }

    public float GetDodgeCDstart()
    {
        return RollDelayStart;
    }

    public static PlayerMovement GetPlayer()
    {
        return instance;
    }

}
