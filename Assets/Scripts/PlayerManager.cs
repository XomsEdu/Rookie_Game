using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Vector2 moveDirection;
    InputManager inputManager;
    Rigidbody2D rigidbody;
    private bool isGrounded;
    
    public LayerMask ground;

    public float moveSpeed;
    public float jumpForce;
    public int jumpAmount = 2;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private int jumpCount;


    public Vector2 groundBoxCast;
    public float castDistance;

    private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

    private void Update()
        {
            if (rigidbody.velocity.y < 0) 
                {
                    rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
                }
            else if (rigidbody.velocity.y > 0 && !Input.GetButton ("Jump")) 
                {
                    rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier) * Time.deltaTime;
                }
        }

    private void FixedUpdate()
        {
            PlayerMovement();
            GroundCheck();
            PlayerJump();
            //playerActions...
        }

    private void PlayerMovement()
        {
            moveDirection = inputManager.movementInput;
            moveDirection.Normalize();
            moveDirection.y = 0;

            rigidbody.velocity = new Vector2(moveDirection.x * moveSpeed, rigidbody.velocity.y);
        
        }

    private void OnDrawGizmos()
        {
            Vector2 boxOrigin = transform.position;
            Vector2 boxDirection = -transform.up;

            // Draw a wireframe box to represent the ground check area
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(boxOrigin - new Vector2(0f, castDistance / 2f), groundBoxCast);
        }

    private void GroundCheck()
        {
            Vector2 boxOrigin = transform.position;
            Vector2 boxDirection = -transform.up;

            RaycastHit2D hit = Physics2D.BoxCast(boxOrigin, groundBoxCast, 0f, boxDirection, castDistance, ground);

            if (hit.collider != null)
            {
                isGrounded = true;
                jumpCount = jumpAmount;
            }
            else
            {
                isGrounded = false;
            }
        }


    private void PlayerJump()
        {
            if (inputManager.isJump && jumpCount > 0)
                {
                    rigidbody.velocity = Vector2.up * jumpForce;

                    jumpCount -= 1;

                    inputManager.isJump = false;
                }
        }
}
