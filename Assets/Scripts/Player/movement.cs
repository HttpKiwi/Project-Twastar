using System;
using UnityEngine;

namespace Player 
{
    public class PlayerMovement : MonoBehaviour
    {
        public float horizontalMoveSpeed = 5f;
        public Rigidbody2D rb;
    
        private float _horizontalInput = 5f;
        [SerializeField] private float _scale = 2f;
    
        void Update()
        {
            Flip();
            // Get input from keyboard
            _horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        }
        
        void FixedUpdate()
        {
            // Apply movement to Rigidbody2D
            rb.linearVelocity = new Vector2(_horizontalInput * horizontalMoveSpeed, rb.linearVelocity.y);
        }
    
        private void Flip()
        {
            transform.localScale = _horizontalInput switch
            {
                < 0 => new Vector2(_scale, transform.localScale.y),
                > 0 => new Vector2(-_scale, transform.localScale.y),
                _ => transform.localScale
            };
        }
    }
}

