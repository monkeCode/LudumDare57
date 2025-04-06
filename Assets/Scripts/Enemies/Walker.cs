using Interfaces;
using UnityEngine;

namespace Enemies
{
    public class Walker : MonoBehaviour, IDamageable
    {
        [SerializeField] private uint health = 50;
        [SerializeField] private uint damage = 5;
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private float platformAttackRangeModifier = 7;
        [SerializeField] private float attackDelay = 3f;
        private float lastAttackTime; 
        
        [SerializeField] private float speed = 5f;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float groundCheckRadius = 0.1f;


        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform wallCheck;

        [SerializeField] private float playerTargetChance = 70f;
        [SerializeField] private bool isPlayerTarget;

        private Rigidbody2D rb;
        private Transform targetTransform;
        private IDamageable targetDamageable;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            var value = Random.Range(0, 100);
            isPlayerTarget = value < playerTargetChance;
            var target = isPlayerTarget ? Player.Player.Instance.gameObject : Platform.Instance.gameObject;
            targetTransform = target.transform;
            targetDamageable = target.GetComponent<IDamageable>();
            if (!isPlayerTarget)
                attackRange *= platformAttackRangeModifier;

        }

        private void Update()
        {
            Move();
            Jump();
            Attack();
        }
        
        
        private void Move()
        {
            if (NearTarget())
            {
                rb.linearVelocityX = 0;
                return;
            }
            var difToTarget = transform.position.x - targetTransform.transform.position.x;
            var direction = difToTarget > 0 ? -1f : 1f;
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

        private bool NearTarget()
        {
            var difToTarget = transform.position.x - targetTransform.transform.position.x;
            return Mathf.Abs(difToTarget) < attackRange;
        }

        public void Attack()
        {
            if (!NearTarget() || !CanAttack())
                return;
            lastAttackTime = Time.time;
            targetDamageable.TakeDamage(damage);
        }
        
        public bool CanAttack() => Time.time - lastAttackTime > attackDelay;
        public void TakeDamage(uint damage)
        {
            health -= damage;
            if (health <= 0)
                Kill();
        }

        public void Heal(uint heals)
        {
            throw new System.NotImplementedException();
        }

        public void Kill()
        {
            Destroy(gameObject);
        }
    }
}
