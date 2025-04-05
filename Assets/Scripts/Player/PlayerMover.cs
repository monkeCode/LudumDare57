using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    class PlayerMover : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField][Range(0, 10)] float _acceleration = 5f;
        [SerializeField][Range(0, 10)] float _flyAcceleration = 3f;
        [SerializeField][Range(0, 10)] float _maxSpeed = 7f;

        [Header("Jump Settings")]
        [SerializeField][Range(0, 150)] float _jumpForce = 100f;
        [SerializeField][Range(0, 1)] float _jumpCutMultiplier = 0.5f;
        [SerializeField][Range(0, 0.5f)] float _groundCheckRadius = 0.1f;
        [SerializeField] LayerMask _groundLayer;
        [SerializeField] Transform _groundCheck;
        [SerializeField] bool _allowDoubleJump = false;

        private Rigidbody2D _rb;
        private bool _isJumping;
        private bool _canDoubleJump;
        private float _moveInput;

        public bool OnGround
        {
            get
            {
                return Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
            }
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Move(float direction, float speedModifier = 1)
        {
            _moveInput = direction;
            float currentSpeed = _rb.linearVelocityX;
            float targetSpeed = _moveInput * _maxSpeed * speedModifier;
            float accel = OnGround ? _acceleration : _flyAcceleration;

            float speedDiff = targetSpeed - currentSpeed;
            float movement = speedDiff * accel;

            _rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }

        public void Jump()
        {
            if (OnGround)
            {
                PerformJump();
                _canDoubleJump = true;
            }
            else if (_allowDoubleJump && _canDoubleJump)
            {
                PerformJump();
                _canDoubleJump = false;
            }
        }

        private void PerformJump()
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, 0);
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _isJumping = true;
        }

        public void CutJump()
        {
            if (_rb.linearVelocityY > 0 && _isJumping)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocityX, _rb.linearVelocityY * _jumpCutMultiplier);
                _isJumping = false;
            }
        }

        private void FixedUpdate()
        {


            if (_rb.linearVelocityY < 0)
            {
                _isJumping = false;
            }
        }

        private void OnDrawGizmos()
        {
            if (_groundCheck != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
            }
        }
    }
}