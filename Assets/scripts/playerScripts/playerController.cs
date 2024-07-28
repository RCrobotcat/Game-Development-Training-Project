using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public static playerController instance { get; private set; }

    private float horizontal;
    private Vector2 lookDirection = new Vector2(1, 0);
    public float speed = 7f; // Adjustable move speed
    public float jumpForce = 5f; // Adjustable jump force

    private Rigidbody2D rigidbody2d;
    public SpriteRenderer playerSprite;

    bool isGrounded;
    bool doubleJumpUsed;

    private Animator playerAnimator;

    bool isWall;

    public Transform wallCheck;
    public LayerMask wallLayer;
    public float wallSlideSpeed = 2f;
    bool isWallSliding;

    public Vector2 wallJumpSpeed = new Vector2(10, 10);
    public float wallJumpTime = 0.1f;
    bool isWallJumping;
    float checkDistance = 1.0f;

    public float dashDistance = 10f;
    public float dashGap = 1f;
    float dashGapSeconds;
    bool isDashing;

    public Ghost ghost;

    public int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }

    bool isInvincible;
    float invincibleTimer;
    public float timeInvincible = 1f; // Adjustable invincible time

    healthBarForPlayer healthBar;
    HealthSystemForDummies healthSystem;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ResetJump();
        }
    }

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystemForDummies>();
        healthBar = GetComponentInChildren<healthBarForPlayer>();
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();

        ResetJump();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        if (!isDashing)
        {
            rigidbody2d.velocity = new Vector2(horizontal * speed, rigidbody2d.velocity.y);
        }

        if (!Mathf.Approximately(horizontal, 0.0f))
        {
            lookDirection.Set(horizontal, 0);
            lookDirection.Normalize();
        }

        playerAnimator.SetFloat("Speed", rigidbody2d.velocity.magnitude);

        if (horizontal < 0)
        {
            playerSprite.flipX = true;
        }
        else if (horizontal > 0)
        {
            playerSprite.flipX = false;
        }

        handleJump();

        WallSlide();
        handleWallJump();

        if (Mathf.Abs(rigidbody2d.velocity.y) > 0.1f)
        {
            playerAnimator.SetBool("isJumpOrFall", true);
            if (rigidbody2d.velocity.y > 0.1f)
            {
                playerAnimator.SetFloat("JumpOrFall", 1);
            }
            if (rigidbody2d.velocity.y < -0.1f)
            {
                playerAnimator.SetFloat("JumpOrFall", -1);
                isGrounded = false;
            }
        }
        else
        {
            playerAnimator.SetBool("isJumpOrFall", false);
            playerAnimator.SetFloat("JumpOrFall", 0);
        }

        // Gap time between dashes
        if (dashGapSeconds > 0)
        {
            dashGapSeconds -= Time.deltaTime;
        }

        // start the coroutine of dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && horizontal != 0)
        {
            if (dashGapSeconds <= 0)
            {
                StartCoroutine(Dash(horizontal));
                dashGapSeconds = dashGap;
            }
        }

        // Invincible time logic
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }
    }

    private void handleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if ((isGrounded || !doubleJumpUsed) && !isWallSliding)
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);

                if (!isGrounded)
                {
                    playerAnimator.SetTrigger("doubleJump");
                    doubleJumpUsed = true;
                }
                isGrounded = false;
            }
        }
    }

    private void handleWallJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isWallSliding)
            {
                isWallJumping = true;
                doubleJumpUsed = false;
                playerAnimator.SetBool("isWallJumping", true);
                Invoke("stopJump", wallJumpTime);
            }
        }
    }

    private void ResetJump()
    {
        isGrounded = true;
        doubleJumpUsed = false;
    }

    private void UpdateWallCheck()
    {
        Vector2 checkDirection = playerSprite.flipX ? Vector2.left : Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, checkDirection, checkDistance, wallLayer);

        isWall = hit.collider != null;

        /*if (isWall)
        {
            Debug.Log("Wall hit: " + hit.collider.name);
        }*/
    }

    // Slide on the wall
    private void WallSlide()
    {
        UpdateWallCheck();

        if (isWall && !isGrounded && horizontal != 0)
        {
            rigidbody2d.velocity = new Vector2(0, Mathf.Clamp(rigidbody2d.velocity.y, -wallSlideSpeed, float.MaxValue));
            isWallSliding = true;
            playerAnimator.SetBool("isWallSliding", true);
        }
        else
        {
            isWallSliding = false;
            playerAnimator.SetBool("isWallSliding", false);
        }

        if (isWallJumping)
        {
            rigidbody2d.velocity = new Vector2(wallJumpSpeed.x * -horizontal, wallJumpSpeed.y); // Jump off the wall by moving in the opposite direction
            playerAnimator.SetBool("isWallJumping", false);
        }
    }

    private void stopJump()
    {
        isWallJumping = false;
    }

    // Dash Coroutine
    IEnumerator Dash(float direction)
    {
        isDashing = true;
        ghost.makeGhost = true; // Create a ghost object
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0f);
        rigidbody2d.AddForce(new Vector2(dashDistance * direction, 0), ForceMode2D.Impulse);
        float gravity = rigidbody2d.gravityScale;
        rigidbody2d.gravityScale = 0;
        yield return new WaitForSeconds(0.2f);
        isDashing = false;
        ghost.makeGhost = false;
        rigidbody2d.gravityScale = gravity;
        dashGapSeconds = dashGap;
    }

    public void ChangeHealth(int amount)
    {
        /*Debug.Log("ChangeHealth: " + amount);*/
        if (amount < 0)
        {
            if (isInvincible)
                return;

            playerAnimator.SetTrigger("isHit");
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        healthSystem.AddToCurrentHealth(amount);
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        /*Debug.Log(currentHealth);*/

        if (currentHealth <= 0)
        {
            healthSystem.Kill();
        }
    }
}