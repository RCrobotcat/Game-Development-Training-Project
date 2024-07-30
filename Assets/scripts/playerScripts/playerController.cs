using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public static playerController instance { get; private set; }

    private float horizontal;
    public Vector2 lookDirection = new Vector2(1, 0);
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

    public float dashDistance = 20f;
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

    float gapBetweenUseItemTimer;
    public float timeGapBetweenUseItem = 1f; // Adjustable gap time between using items

    public healthBarForPlayer healthBar;
    public HealthSystemForDummies healthSystem;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ResetJump();
        }

        if (collision.gameObject.CompareTag("Monster"))
        {
            ChangeHealth(-1);
        }
    }

    private void Awake()
    {
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

        // Use item logic, contains the gap time between using items
        if (gapBetweenUseItemTimer > 0)
        {
            gapBetweenUseItemTimer -= Time.deltaTime;
        }

        handleItemUsed();

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

    IEnumerator Dash(float direction)
    {
        isDashing = true;
        isInvincible = true;
        ghost.makeGhost = true;
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
        rigidbody2d.AddForce(new Vector2(dashDistance * direction, 0), ForceMode2D.Impulse);
        float originalGravity = rigidbody2d.gravityScale;
        rigidbody2d.gravityScale = 0;

        yield return null;  // Wait for one frame to let physics engine update

        float dashTime = 0.2f;
        float elapsedTime = 0f;

        // Ignore collisions with monsters
        while (elapsedTime < dashTime)
        {
            elapsedTime += Time.deltaTime;
            Collider2D[] hits_1 = Physics2D.OverlapCircleAll(transform.position, 1.0f);
            foreach (var hit in hits_1)
            {
                if (hit.CompareTag("Monster"))
                {
                    Collider2D monsterCollider = hit.GetComponent<Collider2D>();
                    if (monsterCollider != null)
                    {
                        Physics2D.IgnoreCollision(monsterCollider, GetComponent<Collider2D>(), true);
                    }
                }
            }
            yield return null;
        }

        isDashing = false;
        ghost.makeGhost = false;
        isInvincible = false;
        rigidbody2d.gravityScale = originalGravity;

        // Ensure collisions are re-enabled
        Collider2D[] hits_2 = Physics2D.OverlapCircleAll(transform.position, 5.0f);
        foreach (var hit in hits_2)
        {
            if (hit.CompareTag("Monster"))
            {
                Collider2D monsterCollider = hit.GetComponent<Collider2D>();
                if (monsterCollider != null)
                {
                    Physics2D.IgnoreCollision(monsterCollider, GetComponent<Collider2D>(), false);
                }
            }
        }
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

    private void handleItemUsed()
    {
        if (gapBetweenUseItemTimer > 0)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InventoryManager.instance.useItem(0);
            gapBetweenUseItemTimer = timeGapBetweenUseItem;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            InventoryManager.instance.useItem(1);
            gapBetweenUseItemTimer = timeGapBetweenUseItem;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            InventoryManager.instance.useItem(2);
            gapBetweenUseItemTimer = timeGapBetweenUseItem;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            InventoryManager.instance.useItem(3);
            gapBetweenUseItemTimer = timeGapBetweenUseItem;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            InventoryManager.instance.useItem(4);
            gapBetweenUseItemTimer = timeGapBetweenUseItem;
        }
    }
}