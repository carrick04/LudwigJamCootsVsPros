using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallclimbandjump : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpDuration = 0.25f;
    [SerializeField] private float wallJumpForce = 7f;
    [SerializeField] private float wallJumpDuration = 0.5f;

    private Rigidbody2D rb;
    private bool isClimbing = false;
    private bool isJumping = false;
    private float horizontalMovement;
    private float jumpTimer = 0f;
    private float wallJumpTimer = 0f;
    private bool wallJumping = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        // Check if the character is touching a wall
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 0.5f, LayerMask.GetMask("Wall"));
        if (hit.collider != null && !isJumping)
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isClimbing)
            {
                // Wall jump
                wallJumping = true;
                isClimbing = false;
                isJumping = true;
                jumpTimer = wallJumpDuration;
                wallJumpTimer = wallJumpDuration;
                rb.velocity = new Vector2(-Mathf.Sign(horizontalMovement) * wallJumpForce, jumpForce);
            }
            else if (!isJumping)
            {
                // Regular jump
                isJumping = true;
                jumpTimer = jumpDuration;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
    }

    private void FixedUpdate()
    {
        // Move the character up or down the wall if they are climbing
        if (isClimbing && !wallJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, horizontalMovement * climbSpeed);
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 1;
        }

        // Wall jump timer
        if (wallJumping && wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.fixedDeltaTime;
        }
        else
        {
            wallJumping = false;
        }

        // Regular jump timer
        if (isJumping && jumpTimer > 0)
        {
            jumpTimer -= Time.fixedDeltaTime;
        }
        else
        {
            isJumping = false;
        }
    }
}
