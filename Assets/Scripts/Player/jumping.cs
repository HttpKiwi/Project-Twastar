using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    void Update()
    {
        
        // Check if player is grounded (using a circle at the player's feet)
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            print("recargando");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        // Variable height: if player releases jump while rising, cut velocity
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y >= 0)
        {
            print("actualizar");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.8f); // soft cut
            // or rb.velocity.y = 0; for a hard cut
        }
    }

    
}