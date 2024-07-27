using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private float horizontal;
    private Vector2 lookDirection = new Vector2(1, 0);
    public float speed = 7f; // Adjustable move speed
    public float jumpForce = 5f; // Adjustable jump force

    private Rigidbody2D rigidbody2d;
    private SpriteRenderer playerSprite;

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
    float checkDistance = 0.8f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ResetJump();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        ResetJump();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        rigidbody2d.velocity = new Vector2(horizontal * speed, rigidbody2d.velocity.y);

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
            }
        }
        else
        {
            playerAnimator.SetBool("isJumpOrFall", false);
            playerAnimator.SetFloat("JumpOrFall", 0);
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

    // 墙壁上下滑
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
            rigidbody2d.velocity = new Vector2(wallJumpSpeed.x * -horizontal, wallJumpSpeed.y); // 反向跳跃
            playerAnimator.SetBool("isWallJumping", false);
        }
    }

    private void stopJump()
    {
        isWallJumping = false;
    }
}