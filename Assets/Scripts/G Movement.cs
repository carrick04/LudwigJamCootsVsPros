using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float rollSpeedBoost = 10f;
    public float rollDuration = 0.5f;
    public float rollCooldown = 1f;
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;
    public float coyoteTime = 0.1f;
    public LayerMask Wall;
    public float doubleJumpForce = 7f;
    bool isJumping = false;
    bool isFalling = false;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isFacingRight = true;
    private bool isRolling = false;
    private bool isOnWall;
    private float coyoteTimeCounter;
    private float rollTimer;
    private float rollCooldownTimer;
    public float fastFallGravityScale = 4f;
    private float originalGravityScale;

    public float wallJumpForce = 10f;
    public float wallSlideSpeed = 1f;
    public float wallStickTime = 0.25f;
    public float wallJumpTime = 0.5f;
    private float wallStickCounter;
    private float wallJumpCounter;
    private bool isWallSliding;
    private int wallDirX;
    private float wallDirectionChangeTime;
    public bool isWallSticking;
    float wallStickEndTime;
    float wallStickStartTime;
    public BoxCollider2D boxCollider1;
    public BoxCollider2D boxCollider2;
    public Vector2 gCheckSize;

    public Animator anim;
    public float animationBufferTime = 0.1f;
    //public Animator anim;
    public Animator animator;

    private float lastStateChangeTime = 0f;
    float previousYVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coyoteTimeCounter = coyoteTime;
        rollCooldownTimer = 0f;
        originalGravityScale = rb.gravityScale;


    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapBox(groundCheck.position, gCheckSize, 0 , groundMask);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Move left or right
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (!isRolling)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
        }
        else
        {
            rb.velocity = new Vector2(moveInput * moveSpeed * rollSpeedBoost, rb.velocity.y);
        }

        // Flip sprite direction based on movement
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }

        // Jump
        if (Input.GetButtonDown("Jump") && (isGrounded || coyoteTimeCounter > 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }


        if (!isGrounded)
        {
            if (Input.GetKey(KeyCode.S))
            {
                rb.gravityScale = fastFallGravityScale;
            }
            else
            {
                rb.gravityScale = originalGravityScale;
            }
        }
       
        // Apply variable jump physics
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }



        if (rb.velocity.y > 0)
        {
            isJumping = true;
            isFalling = false;
        }
        else if (rb.velocity.y < 0 && rb.velocity.y < previousYVelocity)
        {
            isFalling = true;
            isJumping = false;
        }
        /*
        previousYVelocity = rb.velocity.y;

        if (isJumping)
        {
            if (Time.time - lastStateChangeTime >= animationBufferTime)
            {
                anim.SetFloat("airV", rb.velocity.y);
                Debug.Log("Up");
                isFalling = false;
            }
        }
        else if (isFalling)
        {
            if (Time.time - lastStateChangeTime >= animationBufferTime)
            {
                anim.SetFloat("airV", 0);
                anim.SetBool("Fall", true);
                isJumping = false;
            }
        }

        if (isGrounded)
        {
            anim.SetFloat("airV", 0);
            anim.SetBool("Fall", false);
            anim.SetBool("Jump", false);
        }
        */


        //Animations
        if (rb.velocity.y > 0.01f || rb.velocity.y < -0.01f)
        {
            if (rb.velocity.y > 0.1f)
            {
                animator.SetBool("Jump", true);
            }
            if (rb.velocity.y < -0.1f)
            {
                animator.SetBool("Fall", true);
            }
            if (rb.velocity.y < 0.01f)
            {
                animator.SetBool("Jump", false);
            }
            if (rb.velocity.y > -0.01f)
            {
                animator.SetBool("Fall", false);
            }
        }
        if (rb.velocity.y == 0f && isGrounded)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Fall", false);
        }
        Debug.Log(rb.velocity.y);
        // Roll
        if (rollCooldownTimer <= 0f && InputManagerScript.instance.roll.performed && isGrounded && !isRolling)
        {
            isRolling = true;
            rollTimer = rollDuration;
            rollCooldownTimer = rollCooldown;
        }

        if (isRolling)
        {
            rollTimer -= Time.deltaTime;
            boxCollider1.enabled = false;

            if (rollTimer <= 0)
            {
                isRolling = false;
            }
        }
        if (!isRolling)
        {
            boxCollider1.enabled = true;
        }

        if (rollCooldownTimer > 0f)
        {
            rollCooldownTimer -= Time.deltaTime;
        }

      

        /*
                // Wall jump and wall stick

                isOnWall = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, 0.5f, Wall) || Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, 0.5f, Wall);

                if (isOnWall && !isGrounded)
                {
                    // Determine wall direction
                    wallDirX = (Physics2D.Raycast(transform.position, Vector2.right, 0.5f, Wall)) ? 1 : -1;

                    // Stick to wall if player is pressing direction key towards wall
                    if ((Input.GetKey(KeyCode.D) && wallDirX == 1) || (Input.GetKey(KeyCode.A) && wallDirX == -1))
                    {
                        if (isWallSticking)
                        {
                            if (Time.time > wallStickEndTime)
                            {
                                isWallSticking = false;
                            }
                            else if (Input.GetButtonDown("Jump"))
                            {
                                rb.velocity = new Vector2(wallDirX * wallJumpForce, jumpForce);
                                isWallSticking = false;
                            }
                        }
                        else
                        {
                            isWallSticking = true;
                            wallStickStartTime = Time.time;
                            wallStickEndTime = Time.time + wallStickTime;
                        }

                        isWallSliding = true;
                    }
                    else
                    {
                        isWallSliding = false;
                    }
                }

                if (isWallSliding)
                {
                    rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));

                    if (isWallSticking)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }

                    if (Input.GetButtonDown("Jump"))
                    {
                        if (isWallSticking)
                        {
                            rb.velocity = new Vector2(wallDirX * wallJumpForce, jumpForce);
                            isWallSticking = false;
                        }
                        else if (wallJumpCounter > 0)
                        {
                            rb.velocity = new Vector2(-wallDirX * wallJumpForce, jumpForce);
                            wallJumpCounter = 0;
                            isWallSliding = false;
                        }
                    }

                    wallJumpCounter = Mathf.Max(wallJumpCounter - Time.deltaTime, 0);
                }

                if (!isWallSticking && !isGrounded)
                {
                    wallJumpCounter = 0;
                }

        */

        void ResetGravityScale()
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = originalGravityScale;
        }


        void Flip()
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
            if (isOnWall)
            {
                wallDirX = isFacingRight ? -1 : 1;
            }

        }

       
        void EnableCollider()
        {
            // Enable box collider
            boxCollider1.enabled = true;
        }

      
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, gCheckSize);
    }
}

   


