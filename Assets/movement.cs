using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalMoveSpeed = 5f;
    public Rigidbody2D rb;

    private float horizontalInput;

    void Update()
    {
        // Get input from keyboard
        horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
    }

    void FixedUpdate()
    {
        // Apply movement to Rigidbody2D
        rb.linearVelocity = new Vector2(horizontalInput * horizontalMoveSpeed, rb.linearVelocity.y);
    }
}
