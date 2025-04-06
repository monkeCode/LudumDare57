using Interfaces;
using UnityEngine;

namespace Enemies
{
    public class Walker : MonoBehaviour, IDamageable
    {
        [SerializeField] private float speed = 5f;

        [SerializeField] private float jumpHeight;
        [SerializeField] private float groundCheckRadius = 0.1f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform wallCheck;

        private Rigidbody2D rb;
        private Player.Player player;
        private Platform platform; 

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            player = Player.Player.Instance;
            platform = Platform.Instance;
        }

        private void Update()
        {
            Move();
            Jump();
        }
        
        
        private void Move()
        {
            var difToTarget = transform.position.x - player.transform.position.x;
            var direction = difToTarget > 0 ? -1f : 1f;
            if (Mathf.Abs(difToTarget) < 0.5)
            {
                rb.linearVelocityX = 0;
                return;
            }
            rb.linearVelocityX = direction * speed;
            
            FlipPosition(direction);
        }

        private void FlipPosition(float direction)
        {
            transform.localScale = new Vector2(direction, 1);
        }

        private void Jump()
        {
            if (OnGround && NearWall)
            {
                PerformJump();
            }
        }
        private void PerformJump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, 0);
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }
    
        private bool OnGround => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        private bool NearWall => Physics2D.OverlapCircle(wallCheck.position, groundCheckRadius, groundLayer);

        public void Attack()
        {
            throw new System.NotImplementedException();
        }
        public void TakeDamage(uint damage)
        {
            throw new System.NotImplementedException();
        }

        public void Heal(uint heals)
        {
            throw new System.NotImplementedException();
        }

        public void Kill()
        {
            throw new System.NotImplementedException();
        }
    }
}
