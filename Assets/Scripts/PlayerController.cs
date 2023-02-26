using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[Header("References")]
	private Rigidbody2D rb;
	public Animator animator;

	[Header("Movement")]
	public float moveSpeed;
	public float acceleration;
	public float deceleration;
	public float airAccel; //multiplier when player in air
	public float airDecel; //multiplier when player in air
	public float velPower;
	[Space(10)]
	public float frictionAmount;
	[Space(10)]
	private Vector2 moveInput;
	private Vector2 lastMoveInput;
	public bool canMove = true;
	public bool canWallJump = true;
	public bool canClimb = true;
	public float horizontal;
	public float vertical;

	[Header("Jump")]
	public float jumpForce;
	public Vector2 wallJumpForce;
	[Range(0, 1)]
	public float jumpCutMultiplier;
	public float wallJumpStopRunTime;
	[Space(5)]
	public float fallGravityMultiplier;
	private float gravityScale;
	[Space(5)]
	private bool isJumping;
	private bool jumpInputReleased;
	public bool canJump;
	private int jumpCount;
	public int numOfJumps;
	public bool floatModeOn;
	private bool floating;
	public bool doubleJumpOn;

	[Header("Climb & Slide")]
	public float slideForce;
	private bool isSliding;
	public float climbSpeed;

	[Header("Checks")]
	public Transform groundCheckPoint;
	public Vector2 groundCheckSize;
	[Space(5)]
	public Transform frontWallCheckPoint;
	public Transform backWallCheckPoint;
	public Vector2 wallCheckSize;
	[Space(10)]
	public float jumpCoyoteTime;
	private float lastGroundedTime;
	[Space(5)]
	public float jumpBufferTime;
	private float lastJumpTime;
	[Space(5)]
	public float wallJumpCoyoteTime;
	private float lastOnFrontWallTime;
	private float lastOnBackWallTime;
	[Space(10)]
	private bool isFacingRight = true; //sometimes I like to make a ReadOnly attribute to display private varibles like this, allowing for info to be layed out nicer in the inspector

	[Header("Layers & Tags")]
	public LayerMask groundLayer;
	public LayerMask jumpableWallLayer;

	[Header("Roll")]
	private bool isRolling;
	public BoxCollider2D boxCollider1;
	private float rollCooldownTimer;
	public float rollTimer;
	public float rollDuration;
	public float rollCooldown;
	public Vector2 rollForce;
	private bool rollflipped;
	private bool rollhasBeenReleased;
	private bool jumphasBeenReleased;
	private Vector2 lastCheckpoint;
	private void Start()
	{
		/*
		 - retrieves rigidbody
		 - if you want the player to have more functionality in future eg: combat, more movement options, etc. 
		 - I would recommend creating a seperate Player Class and using that to hold all player info such as the rigidbody and make seperate movement, combat, classes for specific functions
		 - Highly recommed looking into abstacrtion, decoupling and inheritance if you're working on a large project
		*/
		rb = GetComponent<Rigidbody2D>();

		gravityScale = rb.gravityScale;

		jumpCount = numOfJumps;

		isRolling = false;
		rollflipped = false;
		rollhasBeenReleased = true;
		jumphasBeenReleased = true;
		lastCheckpoint = transform.position;
	}

	private void Update()
	{
		//Debug.Log(rb.velocity.y);
		#region Inputs

		moveInput.x = InputManagerScript.instance.Mover().x;
		moveInput.y = InputManagerScript.instance.Mover().y;

		animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
		
		#endregion

		#region Run
		if (moveInput.x != 0)
			lastMoveInput.x = moveInput.x;
		if (moveInput.y != 0)
			lastMoveInput.y = moveInput.y;

		if ((lastMoveInput.x > 0 && !isFacingRight) || (lastMoveInput.x < 0 && isFacingRight))
		{
			Turn();
			isFacingRight = !isFacingRight;
		}
		#endregion

		#region Ground
		//checks if set box overlaps with ground
		if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer) && !isJumping)
		{
			//resets countdown timer
			Debug.Log("ground");
			lastGroundedTime = jumpCoyoteTime;
			jumpCount = numOfJumps;
			
			//animator.SetBool("Jump", false);
		}
		#endregion

		#region Wall
		//checks if set box overlaps with wall in front of player
		if (lastGroundedTime <= 0)
		{
			if (Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer))
			{
				//resets countdown timer
				lastOnFrontWallTime = wallJumpCoyoteTime;
				lastOnBackWallTime = 0;
			}
			else if (Physics2D.OverlapBox(backWallCheckPoint.position, wallCheckSize, 0, groundLayer))
			{
				//resets countdown timer
				lastOnBackWallTime = wallJumpCoyoteTime;
				lastOnFrontWallTime = 0;
			}
		}

		if (lastOnFrontWallTime > 0 || lastOnBackWallTime > 0)
			isSliding = true;
		else
			isSliding = false;
		#endregion

		#region Jump
		//checks if the player is grounded or falling and that they have released jump
		if (rb.velocity.y <= 0)
		{
			//if so we are no longer jumping and could jump again
			isJumping = false;
		}
        
		if (canJump && jumphasBeenReleased)
		{
			jumphasBeenReleased = false;
			Debug.Log("Can jump");
			if (InputManagerScript.instance.jump.started || InputManagerScript.instance.jump.performed)
			{
				//OnJump();
				lastJumpTime = jumpBufferTime;
				jumpInputReleased = true;
				Debug.Log("jump");

				if (floatModeOn)
				{
					Debug.Log("Float Mode On");
					floating = true;
				}
				Debug.Log(lastJumpTime + " " + isJumping + " " + jumpInputReleased);

				if (lastJumpTime > 0 && !isJumping && jumpInputReleased)
				{
					Debug.Log("1");
					if (lastGroundedTime > 0)
					{
						lastGroundedTime = 0;
						Jump(jumpForce);
						jumpCount--;						
						Debug.Log("2");
					}
					else if (lastOnFrontWallTime > 0 && canWallJump)
					{
						lastOnFrontWallTime = 0;
						WallJump(wallJumpForce.x, wallJumpForce.y);
						StopMovement(wallJumpStopRunTime);
						Debug.Log("3");
					}
					else if (lastOnBackWallTime > 0 && canWallJump)
					{
						lastOnBackWallTime = 0;
						WallJump(-wallJumpForce.x, wallJumpForce.y);
						StopMovement(wallJumpStopRunTime);
						Debug.Log("4");
					}
				}
				else if (doubleJumpOn)
				{
					if (jumpCount > 0)
					{
						lastGroundedTime = 0;
						Jump(jumpForce);
						jumpCount--;
					}
				}
			}
			/*
			if (InputManagerScript.instance.down.ReadValue<float>() < 0.1)
			{
				OnJumpUp();
				Debug.Log("Jump cancelled");
			}
			*/

			
		}

		if (rollCooldownTimer <= 0f && InputManagerScript.instance.roll.performed && !isRolling && rollhasBeenReleased/*&& lastGroundedTime == jumpCoyoteTime*/)
		{
			isRolling = true;
			rollTimer = rollDuration;
			rollCooldownTimer = rollCooldown;
			Debug.Log("roll presed");

			rollhasBeenReleased = false;
		}

		if (InputManagerScript.instance.roll.ReadValue<float>() == 0f)
        {
			rollhasBeenReleased = true;
		}

		if (InputManagerScript.instance.jump.ReadValue<float>() == 0f)
		{
			jumphasBeenReleased = true;
		}

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
		if (rb.velocity.y == 0f)
		{
			animator.SetBool("Jump", false);
			animator.SetBool("Fall", false);
		}

		if (isRolling)
		{
			rollTimer -= Time.deltaTime; 
			boxCollider1.enabled = false;

			if (!rollflipped)
			{

				
				rb.AddForce(rollForce);
				
			}
			else
			{
				
				rb.AddForce(new Vector2(-rollForce.x, rollForce.y));
				
			}

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

		Debug.Log(rb.velocity.y);
		#endregion

		#region Timer
		lastGroundedTime -= Time.deltaTime;
		lastOnFrontWallTime -= Time.deltaTime;
		lastOnBackWallTime -= Time.deltaTime;
		lastJumpTime -= Time.deltaTime;
		#endregion
	}

	private void FixedUpdate()
	{
		#region Run
		if (canMove)
		{
			//calculate the direction we want to move in and our desired velocity
			float targetSpeed = moveInput.x * moveSpeed;
			//calculate difference between current velocity and desired velocity
			float speedDif = targetSpeed - rb.velocity.x;

			//change acceleration rate depending on situation
			float accelRate;
			if (lastGroundedTime > 0)
			{
				accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
			}
			else
			{
				accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration * airAccel : deceleration * airDecel;
			}

			//applies acceleration to speed difference, the raises to a set power so acceleration increases with higher speeds
			//finally multiplies by sign to reapply direction
			float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

			//applies force force to rigidbody, multiplying by Vector2.right so that it only affects X axis 
			rb.AddForce(movement * Vector2.right);
		}

		#endregion

		#region Friction
		//check if we're grounded and that we are trying to stop (not pressing forwards or backwards)
		if (lastGroundedTime > 0 && !isJumping && Mathf.Abs(moveInput.x) < 0.01f)
		{
			//then we use either the friction amount (~ 0.2) or our velocity
			float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
			//sets to movement direction
			amount *= Mathf.Sign(rb.velocity.x);

			//applies force against movement direction
			rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
		}
		#endregion

		#region Climb & Slide
		//check if we're grounded and that we are trying to stop (not pressing forwards or backwards)
		if ((lastOnBackWallTime > 0 || lastOnFrontWallTime > 0) && canClimb)
		{
			float amount = Mathf.Min(Mathf.Abs(rb.velocity.y), Mathf.Abs(slideForce));
			amount *= Mathf.Sign(rb.velocity.y);
			//rb.velocity = new Vector2(0, 0);
			rb.AddForce(Vector2.up * -amount, ForceMode2D.Impulse);


			
			if (Input.GetKey(KeyCode.Z))
			{
				rb.velocity = new Vector2(rb.velocity.x, climbSpeed * moveInput.x);
			}
			
		}
		#endregion

		#region Jump Gravity
		if (!floatModeOn)
		{
			if (rb.velocity.y < 0 && lastGroundedTime <= 0 && !isRolling)
			{
				rb.gravityScale = gravityScale * fallGravityMultiplier;
			}
			else if (InputManagerScript.instance.Mover().y < -0.01)
			{
				rb.gravityScale = gravityScale * fallGravityMultiplier;
			}
			else
			{
				rb.gravityScale = gravityScale;
			}
		}
		else if (floatModeOn)
		{
			if (floating && rb.velocity.y < 0 && lastGroundedTime <= 0 && !isSliding)
			{

				rb.gravityScale = gravityScale / fallGravityMultiplier;

			}
			else if (rb.velocity.y < 0 && lastGroundedTime <= 0 && !isSliding)
			{
				rb.gravityScale = gravityScale * fallGravityMultiplier;
			}
			else
			{
				rb.gravityScale = gravityScale;
			}
		}
		#endregion
	}

	#region Jump
	private void Jump(float jumpForce)
	{

		//apply force, using impluse force mode
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		lastJumpTime = 0;
		isJumping = true;
		jumpInputReleased = false;

	}

	private void WallJump(float jumpForceX, float jumpForceY)
	{
		//flips x force if facing other direction, since when we Turn() our player the CheckPoints swap around

		if (!isFacingRight)
		{
			jumpForceX *= -1;
		}		

		//New Wall Jump
		if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(jumpForceX))
		{
			jumpForceX -= rb.velocity.x;
		}

		//checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
		if (rb.velocity.y + jumpForceY < jumpForceY)
		{
			jumpForceY -= rb.velocity.y;
		}
		rb.velocity = new Vector2(0, rb.velocity.y);

		Vector2 force = new Vector2(jumpForceX, jumpForceY);

		rb.AddForce(force, ForceMode2D.Impulse);

		//Debug.Log("Wall Jump: Facing Right - " + isFacingRight + " Force - " + jumpForceX + ", " + jumpForceY);
	}

	public void OnJump()
	{
		lastJumpTime = jumpBufferTime;
		jumpInputReleased = false;
	}

	public void OnJumpUp()
	{
		if (rb.velocity.y > 0 && isJumping)
		{
			//reduces current y velocity by amount (0 - 1)
			rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
		}

		jumpInputReleased = true;
		lastJumpTime = 0;

		if (floatModeOn)
		{
			floating = false;
		}
	}
	#endregion

	private IEnumerator StopMovement(float duration)
	{
		canMove = false;
		canJump = false;
		yield return new WaitForSeconds(duration);
		canMove = true;
		canJump = true;
	}

	private void Turn()
	{
		//stores scale and flips x axis, flipping the entire gameObject (could also rotate the player)
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
		if (rollflipped)
        {
			rollflipped = false;

		}
		else
        {
			rollflipped = true;
		}		
	}


	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(frontWallCheckPoint.position, wallCheckSize);
		Gizmos.DrawWireCube(backWallCheckPoint.position, wallCheckSize);

	}

	public void KillPlayer()
    {
		transform.position = lastCheckpoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Check")
        {
			lastCheckpoint = new Vector2(collision.transform.position.x, collision.transform.position.y);
		}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Death")
        {
			KillPlayer();

		}
    }
}
