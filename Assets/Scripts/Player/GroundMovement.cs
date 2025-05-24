using UnityEngine;

namespace Player
{
    public class FrogController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float maxMoveSpeed = 50f;
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float friction = 15f;
        [SerializeField] private float crouchSpeedReduction = 0.6f;
        [SerializeField] private float iceSpeedMultiplier = 2f;
        [SerializeField] private float mudSpeedReduction = 0.6f;
        
        [Header("Jump Settings")]
        [SerializeField] private float jumpHeight = 32f;
        [SerializeField] private float jumpCutMultiplier = 0.75f;
        [SerializeField] private float coyoteTime = 0.15f;
        [SerializeField] private float jumpBufferTime = 0.1f;
        
        [Header("Ground Detection")]
        [SerializeField] private LayerMask groundLayer = 1;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.2f;
        
        [Header("Visual")]
        [SerializeField] private float spriteXScale = 1f;
        [SerializeField] private float crouchScale = 0.9f;
        [SerializeField] private float spriteYScale = 1f;
        
        // Components
        private Rigidbody2D rb;
        
        // Input
        private float horizontalInput;
        private bool jumpPressed;
        private bool jumpHeld;
        private float tBagHeld;
        
        // Movement State
        private bool isGrounded;
        private bool wasGrounded;
        private bool isCrouched;
        private bool onIce;
        private bool onMud;
        private float coyoteTimeCounter;
        private float jumpBufferCounter;
        
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteYScale = transform.localScale.y;
        }
        
        void Update()
        {
            GatherInput();
            CheckGroundStatus();
            HandleTimers();
            HandleJump();
            HandleTBag();
            FlipSprite();
        }
        
        void FixedUpdate()
        {
            HandleMovement();
        }
        
        // void OnDrawGizmosSelected()
        // {
        //     if (groundCheck != null)
        //     {
        //         Gizmos.color = isGrounded ? Color.green : Color.red;
        //         Gizmos.DrawCube(
        //             new Vector3(transform.position.x, transform.position.y - 0.766f, transform.position.z),
        //             new Vector3(1.75f, 0.05f, 1f));
        //     }
        // }
        
        private void GatherInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            jumpPressed = Input.GetButtonDown("Jump");
            jumpHeld = Input.GetButton("Jump");
            tBagHeld = Input.GetAxisRaw("Vertical");
        }
        
        private void CheckGroundStatus()
        {
            wasGrounded = isGrounded;

            // Get all colliders under the player
            Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheck.position, new Vector2(1.75f, 0.05f), 0f, groundLayer);

            isGrounded = colliders.Length > 0;
            onIce = false; // Reset ice detection
            onMud = false;
            
            foreach (var col in colliders)
            {
                if (col.CompareTag("Ice"))
                {
                    onIce = true;
                    break; // No need to keep checking
                } 
                else if (col.CompareTag("Mud"))
                {
                    onMud = true;
                    break;
                }
            }
        }
        
        private void HandleTimers()
        {
            if (isGrounded)
                coyoteTimeCounter = coyoteTime;
            else
                coyoteTimeCounter -= Time.deltaTime;
            
            if (jumpPressed)
                jumpBufferCounter = jumpBufferTime;
            else
                jumpBufferCounter -= Time.deltaTime;
        }
        
        private void HandleMovement()
        {

            float baseMoveSpeed = CalculateMovementSpeed(moveSpeed);
            float baseAcceleration = acceleration;
            
            if (isCrouched)
            {
                baseMoveSpeed *= crouchSpeedReduction;
            }
            
            if (onIce)
            {
                baseAcceleration = 1f; // to lose control in ice
            }
            
            float targetSpeed = horizontalInput * baseMoveSpeed;
            float currentSpeed = rb.linearVelocity.x;
            
            float speedDifference = targetSpeed - currentSpeed;
            float movementForce = speedDifference * (Mathf.Abs(horizontalInput) > 0.1f ? baseAcceleration : friction);
            
            movementForce = Mathf.Clamp(movementForce, -maxMoveSpeed, maxMoveSpeed);
            
            rb.AddForce(Vector2.right * movementForce, ForceMode2D.Force);
        }
        
        private void HandleJump()
        {
            float baseMoveSpeed = CalculateMovementSpeed(rb.linearVelocity.x);
            
            baseMoveSpeed = Mathf.Clamp(baseMoveSpeed, -maxMoveSpeed, maxMoveSpeed);
            
            // Can jump if: is grounded, in coyote time, or has a jump buffered
            bool canJump = (isGrounded || coyoteTimeCounter > 0f) && jumpBufferCounter > 0f;
            
            if (canJump)
            {
                float jumpForce = Mathf.Sqrt(jumpHeight * -2f * Physics2D.gravity.y * rb.gravityScale);
                rb.linearVelocity = new Vector2(baseMoveSpeed, jumpForce);
                
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
            }
            
            if (!jumpHeld && rb.linearVelocity.y > 0.5f) 
            {
                rb.linearVelocity = new Vector2(baseMoveSpeed, rb.linearVelocity.y * jumpCutMultiplier);
            }
        }

        private float CalculateMovementSpeed(float baseMoveSpeed)
        {
            
            if (onIce)
            {
                baseMoveSpeed *= iceSpeedMultiplier;
            }
            
            if (onMud)
            {
                baseMoveSpeed *= mudSpeedReduction;
            }
            return baseMoveSpeed;
        }
        
        private void HandleTBag()
        {
            // Can crouch if: is grounded
            if (isGrounded)
            {
                if (tBagHeld < 0)
                {
                    if (transform.localScale != new Vector3(transform.localScale.x, spriteYScale * crouchScale, transform.localScale.z))
                    {
                        transform.localScale = new Vector3(transform.localScale.x, spriteYScale * crouchScale, transform.localScale.z);
                        isCrouched = true;
                    }
                }
                else
                {
                    if (transform.localScale.y != spriteYScale)
                    {
                        transform.localScale = new Vector3(transform.localScale.x, spriteYScale, transform.localScale.z);
                        isCrouched = false;
                    }
                }
            }
        }
        
        private void FlipSprite()
        {
            transform.localScale = horizontalInput switch
            {
                < 0 => new Vector2(spriteXScale, transform.localScale.y),
                > 0 => new Vector2(-spriteXScale, transform.localScale.y),
                _ => transform.localScale
            };
        }
        
        public bool IsGrounded => isGrounded;
        public bool IsMoving => Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        public bool IsFalling => rb.linearVelocity.y < -0.1f;
        public Vector2 Velocity => rb.linearVelocity;
    }
}