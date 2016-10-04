using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {

		//VARIABLES FOR MOOVING 
        [SerializeField] private float m_MaxSpeed = 10;
		[SerializeField] private float m_LowSpeed = 5;     

        [SerializeField] private float m_JumpForce = 800;
		private float jumpHeightMax = 1200;					//Jumptheight when blue
		private float jumpHeightFixed = 800; 				//Jumptheight when white
		private float jumptHeightPurp = 10;

        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

		//COLORS
		private Color red = new Color(0.82F, 0F, 0.2F);			//Red farve
		private Color yellow = new Color(1F, 0.8F, 0.2F);		//Yellow farve
		private Color blue = new Color(0.2F, 0.18F, 1F);		//Blue farve
		private Color green = new Color(0.2F, 0.8F, 0.15F);		//Green farve
		private Color purple = new Color(0.55F, 0.15F, 0.7F);	//Purple farve
		private Color orange = new Color(1F, 0.6F, 0.2F);		//Orange farve

		//FOR DASH
		public DashState dashState;
		public float dashTimer;
		public float maxDash = 20f;
		public Vector2 savedVelocity;

		//FOR HIGHJUMP
		private bool highJumpKeyPressed;

		//FOR STICKY AND WALLJUMP//
		bool touchingWallRight = false; 
		bool touchingWallLeft = false;
		private Transform rightCheck;
		private Transform leftCheck;
		[SerializeField] private LayerMask whatIsWall;
		const float wallTouchRadius = .05f;
		private float jumpPushForce = 20f;
		private float yellowJump = 20f;
		float jumpDuration;
		public float JumpDuration;
		bool canVariableJump = false;
		private new Vector2 amountTomove;
		public float gravity = 3;

		//FOR WALLHOLDING
		private bool isWallHolding = false;

		//FOR GROUND AND CEELING
        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
 
      
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

		//FOR POSSIBLE MATERIAL CHANGE ON BLUE
		public PhysicsMaterial2D bouncy;

        private void Awake()
        {
            //REFERENCES
            m_GroundCheck = transform.Find("GroundCheck");
            m_Anim = GetComponent<Animator>();
			m_Rigidbody2D = GetComponent<Rigidbody2D>();

			rightCheck = transform.Find ("rightCheck");
			leftCheck = transform.Find ("leftCheck");

			highJumpKeyPressed = false;
        }

		public void Move(float move, bool jump)
		{
			
			//only control the player if grounded or airControl is turned on
			if (m_Grounded || m_AirControl)
			{
				
				// The Speed animator parameter is set to the absolute value of the horizontal input.
				m_Anim.SetFloat("Speed", Mathf.Abs(move));
				
				// Move the character
				m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

				// If the input is moving the player right and the player is facing left...
//				if (move > 0 && !m_FacingRight)
//				{
//					// ... flip the player.
//					Flip();
//				}
//				// Otherwise if the input is moving the player left and the player is facing right...
//				else if (move < 0 && m_FacingRight)
//				{
//					// ... flip the player.
//					Flip();
//				}
			}
			// If the player should jump...
			if (m_Grounded && jump && m_Anim.GetBool("Ground"))
			{
				// Add a vertical force to the player.
				m_Grounded = false;
				m_Anim.SetBool("Ground", false);
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			}
		}
		
		//FLIPING THE DIRECTION THE CHARACTER IS FACING
//		private void Flip()
//		{
//			// Switch the way the player is labelled as facing.
//			m_FacingRight = !m_FacingRight;
//			
//			// Multiply the player's x local scale by -1.
//			Vector3 theScale = transform.localScale;
//			theScale.x *= -1;
//			transform.localScale = theScale;
//		}
		//DASH ENUM
		public enum DashState 
		{
			Ready,
			Dashing,
			Cooldown
		}
        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
			//FOR ANIMATION WHEN GROUNDED
            m_Anim.SetBool("Ground", m_Grounded);

            //FOR ANIMATION WHEN JUMPING OR FALLING
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);


			//FOR STICKY AND WALLJUMP//
			//Touching wall right?
			touchingWallRight = Physics2D.OverlapCircle(rightCheck.position, wallTouchRadius, whatIsWall);
			if (touchingWallRight) 
			{
				Debug.Log ("touching right wall");
				m_Grounded = false; 
				touchingWallRight = true;
				if (isWallHolding = true)
				{
					Debug.Log ("WallHoldingRight");
				}
			}


			//Touching wall left?
			touchingWallLeft = Physics2D.OverlapCircle(leftCheck.position, wallTouchRadius, whatIsWall);
			if (touchingWallLeft) 
			{
				Debug.Log ("touching left wall");
				m_Grounded = false; 
				touchingWallLeft = true;
				if (isWallHolding = true)
				{
					Debug.Log ("WallHoldingLeft");
				}
			}

			//BLUE TAG//
			if(gameObject.tag == "blue")
			{
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				Debug.Log ("tagged as blue");

				gameObject.GetComponent<CircleCollider2D>().sharedMaterial = bouncy;
					//(PhysicsMaterial2D)Resources.Load("PhysicMaterials/Bouncy");


				if (!highJumpKeyPressed && Input.GetKeyDown (KeyCode.LeftShift))
				{
					highJumpKeyPressed = true;

					if (highJumpKeyPressed == true)
					{
						m_JumpForce = jumpHeightMax;
					}
					else 
					{
						m_JumpForce = jumpHeightFixed;
					}
				}
				else if (highJumpKeyPressed && Input.GetKeyDown (KeyCode.LeftShift))
				{
					m_JumpForce = jumpHeightFixed;
					highJumpKeyPressed = false;
				}
			}
			//WHITE TAG//
			else if(gameObject.tag == "white")
			{
				Debug.Log ("tagged as white"); 
				m_JumpForce = jumpHeightFixed;
				GetComponent<Rigidbody2D>().gravityScale = 3;
				//			if(Input.GetKeyDown (KeyCode.Space))
				//			{
				//				GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
				//			}
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else if(gameObject.tag == "Untagged")
			{
				Debug.Log ("not tagged yet");
			}
			//RED TAG//
			else if(gameObject.tag == "red")
			{
				Debug.Log ("tagged as red");
				//DASH//
				switch (dashState) 
				{
				case DashState.Ready:
					var isDashKeyDown = Input.GetKeyDown (KeyCode.LeftShift);
					if(isDashKeyDown)
					{
						savedVelocity = m_Rigidbody2D.velocity;
						m_Rigidbody2D.velocity =  new Vector2(m_Rigidbody2D.velocity.x * 20f, m_Rigidbody2D.velocity.y);
						dashState = DashState.Dashing;
					}
					break;
				case DashState.Dashing:
					dashTimer += Time.deltaTime * 3;
					if(dashTimer >= maxDash)
					{
						dashTimer = maxDash;
						m_Rigidbody2D.velocity = savedVelocity;
						dashState = DashState.Cooldown;
					}
					break;
				case DashState.Cooldown:
					dashTimer -= Time.deltaTime;
					if(dashTimer <= 0)
					{
						dashTimer = 0;
						dashState = DashState.Ready;
					}
					break;
				}
			}
			//YELLOW TAG//
			else if(gameObject.tag == "yellow")
			{
				Debug.Log ("tagged as yellow");

				if (isWallHolding) 
				{
//					m_Rigidbody2D.velocity = new Vector2(
					amountTomove.x = 0;
					if (Input.GetAxisRaw("Vertical") != -1) 
					{
						amountTomove.y = 1;	
					}
				}
					//FOR STICKY AND WALLJUMP//
				if (touchingWallRight && Input.GetButtonDown ("Jump")) 
				{
					m_Rigidbody2D.velocity = new Vector2(this.jumpPushForce * -1, this.yellowJump);			
					//m_Rigidbody2D.AddForce (new Vector2(this.jumpPushForce * -1, this.m_JumpForce));
					jumpDuration = 0.0f;
					canVariableJump = true;
						
				}
				if (touchingWallLeft && Input.GetButtonDown ("Jump")) 
				{
					m_Rigidbody2D.velocity = new Vector2(this.jumpPushForce * 1, this.yellowJump);
					//m_Rigidbody2D.AddForce(new Vector2(this.jumpPushForce * 1, this.m_JumpForce));
					jumpDuration = 0.0f;
					canVariableJump = true;
						
				}
				else if (canVariableJump)
				{
					jumpDuration += Time.deltaTime;
						
					if(jumpDuration < this.JumpDuration /1000)
					{
						m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, this.m_JumpForce);
					}
				}

			}
			//PURPLE TAG//
			else if(gameObject.tag == "purple")
			{
				Debug.Log ("tagged as purple");
				GetComponent<Rigidbody2D>().gravityScale = -3;
				m_JumpForce = jumpHeightFixed;
				if(Input.GetKeyDown (KeyCode.Space))
				{
					GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -jumptHeightPurp);
				}
			}
			//ORANGE TAG//
			else if(gameObject.tag == "orange")
			{
				Debug.Log ("target is orange");
				GetComponent<Rigidbody2D>().gravityScale = 3;
				gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
				
			}
			//GREEN TAG//
			else if(gameObject.tag == "green")
			{
				Debug.Log ("tagged as green");
				gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
				GetComponent<Rigidbody2D>().gravityScale = 3;
				m_JumpForce = jumpHeightFixed;
			}
        }

		void OnCollisionEnter2D(Collision2D col)
		{
			//BLUE COLLISION//
			if(col.gameObject.tag == "blueBox")
			{
				if(gameObject.tag == "red")
				{
					gameObject.GetComponent<Renderer>().material.color = purple;
					gameObject.tag = "purple";
				}
				else if (gameObject.tag == "yellow")
				{
					gameObject.GetComponent<Renderer>().material.color = green;
					gameObject.tag = "green";
				}
				
				else if (gameObject.tag != "blue" && gameObject.tag != "green" && gameObject.tag != "red" && gameObject.tag != "purple" && gameObject.tag != "orange" && gameObject.tag != "yellow")
				{
					gameObject.GetComponent<Renderer>().material.color = blue;
					gameObject.tag = "blue";
				}
				
			}
			//WATER COLLISION//
			if(col.gameObject.tag == "Water")
			{
				
				gameObject.GetComponent<Renderer>().material.color = Color.white;
				gameObject.tag = "white";
			}
			//RED COLLISION//
			if(col.gameObject.tag == "redBox")
			{	
				if(gameObject.tag == "blue")
				{
					gameObject.GetComponent<Renderer>().material.color = purple;
					gameObject.tag = "purple";
				}
				else if (gameObject.tag != "blue" && gameObject.tag != "green" && gameObject.tag != "red" && gameObject.tag != "purple" && gameObject.tag != "orange" && gameObject.tag != "yellow")
				{
					gameObject.GetComponent<Renderer>().material.color = red;
					gameObject.tag = "red";
				}
				else if (gameObject.tag == "yellow")
				{
					gameObject.GetComponent<Renderer>().material.color = orange;
					gameObject.tag = "orange";
				}
			}
			//BLACK COLLISION//
			if(col.gameObject.tag == "blackBox")
			{
				Destroy(gameObject);
				Debug.Log ("You are dead!");
			}
			//YELLOW COLLISION//
			if(col.gameObject.tag == "yellowBox")
			{
				if(gameObject.tag == "blue")
				{
					gameObject.GetComponent<Renderer>().material.color = green;
					gameObject.tag = "green";
				}
				else if (gameObject.tag != "blue" && gameObject.tag != "green" && gameObject.tag != "red" && gameObject.tag != "purple" && gameObject.tag != "orange" && gameObject.tag != "yellow")
				{
					gameObject.GetComponent<Renderer>().material.color = yellow;
					gameObject.tag = "yellow";
				}
				else if (gameObject.tag == "red")
				{
					gameObject.GetComponent<Renderer>().material.color = orange;
					gameObject.tag = "orange";
				}
				
			}
			//ORANGE COLLISION//
			if(col.gameObject.tag == "orangeBox")
			{
				if (gameObject.tag != "blue" && gameObject.tag != "green" && gameObject.tag != "red" && gameObject.tag != "purple" && gameObject.tag != "orange" && gameObject.tag != "yellow")
				{
					gameObject.GetComponent<Renderer>().material.color = orange;
					gameObject.tag = "orange";
				}
			}
			//ORANGE DESTROY COLLISION//
			if(col.gameObject.tag == "orangeDestroy")
			{
				if(gameObject.tag =="orange")
				{
					Destroy(col.gameObject);
				}
			}
			//PURPLE COLLISION//
			if(col.gameObject.tag == "purpleBox")
			{
				if (gameObject.tag != "blue" && gameObject.tag != "green" && gameObject.tag != "red" && gameObject.tag != "purple" && gameObject.tag != "orange" && gameObject.tag != "yellow")
				{
					gameObject.GetComponent<Renderer>().material.color = purple;
					gameObject.tag = "purple";
				}
			}
			//GREEN COLLISION//
			if(col.gameObject.tag == "greenBox")
			{
				if (gameObject.tag != "blue" && gameObject.tag != "green" && gameObject.tag != "red" && gameObject.tag != "purple" && gameObject.tag != "orange" && gameObject.tag != "yellow")
				{
					gameObject.GetComponent<Renderer>().material.color = green;
					gameObject.tag = "green";
				}
			}

			//ON COLLISION WITH A MOVING PLATFORM
			if(col.transform.tag == "movingPlatform")
			{
				transform.parent = col.transform;


			}
		}

		//ON COLLISIONEXIT  WITH A MOVING PLATFORM
		void OnCollisionExit2D(Collision2D col)
		{
			if(col.transform.tag == "movingPlatform");
			{
				transform.parent = null;
			}
		}
        
    }
}
