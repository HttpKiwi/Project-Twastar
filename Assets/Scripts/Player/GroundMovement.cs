using UnityEngine;

namespace Player
{
    public class FrogController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float friction = 15f;
        
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
        [SerializeField] private float spriteScale = 1f;
        
        // Components
        private Rigidbody2D rb;
        
        // Input
        private float horizontalInput;
        private bool jumpPressed;
        private bool jumpHeld;
        
        // Movement State
        private bool isGrounded;
        private bool wasGrounded;
        private float coyoteTimeCounter;
        private float jumpBufferCounter;
        
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        
        void Update()
        {
            GatherInput();
            CheckGroundStatus();
            HandleTimers();
            HandleJump();
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
        }
        
        private void CheckGroundStatus()
        {
            wasGrounded = isGrounded;
            isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector3(1.75f, 0.05f, 1f), 0f);
            
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
            float targetSpeed = horizontalInput * moveSpeed;
            float currentSpeed = rb.linearVelocity.x;
            
            float speedDifference = targetSpeed - currentSpeed;
            float movementForce = speedDifference * (Mathf.Abs(horizontalInput) > 0.1f ? acceleration : friction);
            
            rb.AddForce(Vector2.right * movementForce, ForceMode2D.Force);
        }
        
        private void HandleJump()
        {
            // Can jump if: is grounded, in coyote time, or has a jump buffered
            bool canJump = (isGrounded || coyoteTimeCounter > 0f) && jumpBufferCounter > 0f;
            
            if (canJump)
            {
                float jumpForce = Mathf.Sqrt(jumpHeight * -2f * Physics2D.gravity.y * rb.gravityScale);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
            }
            
            if (!jumpHeld && rb.linearVelocity.y > 0.5f) 
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
        }
        
        private void FlipSprite()
        {
            if (Mathf.Abs(horizontalInput) > 0.1f)
            {
                float direction = horizontalInput > 0 ? -1 : 1;
                transform.localScale = new Vector2(direction * spriteScale, spriteScale);
            }
        }
        
        public bool IsGrounded => isGrounded;
        public bool IsMoving => Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        public bool IsFalling => rb.linearVelocity.y < -0.1f;
        public Vector2 Velocity => rb.linearVelocity;
    }
}