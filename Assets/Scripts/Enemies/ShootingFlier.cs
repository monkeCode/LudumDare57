using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Weapons;

namespace Enemies
{
    public class ShootingFlier : MonoBehaviour, IEnemy
    {
        [SerializeField] private uint health = 50;
        [SerializeField] private uint damage = 5;
        [SerializeField] private float bulletSpeed = 20f;
        [SerializeField] private float attackRange = 20f;
        [SerializeField] private float attackDelay = 3f;
        private float currentDelay;

        [SerializeField] private Bullet bullet;
        
        [SerializeField] private float speed = 5f;

        [SerializeField] private float playerTargetChance = 70f;
        [SerializeField] private bool isPlayerTarget;

        private Rigidbody2D rb;
        private NavMeshAgent navMeshAgent;
        private Transform targetTransform;
        private IDamageable targetDamageable;

        private Vector2 flyAwayDirection;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
            navMeshAgent.speed = speed;
            
            var value = Random.Range(0, 100);
            isPlayerTarget = value < playerTargetChance;
            var target = isPlayerTarget ? Player.Player.Instance.gameObject : Platform.Instance.gameObject;
            targetTransform = target.transform;
            targetDamageable = target.GetComponent<IDamageable>();
        }

        private void Update()
        {
            currentDelay -= Time.deltaTime;
            if (!CanAttack())
            {
                FlyAway();    
                return;
            }
            Move();
            Attack();
        }
        
        
        private void Move()
        {
            navMeshAgent.SetDestination(targetTransform.position);
        }

        private bool NearTarget(float distance)
        {
            var difToTarget = transform.position - targetTransform.transform.position;
            return difToTarget.magnitude < distance;
        }

        public void Attack()
        {
            if (!NearTarget(attackRange) || !CanAttack())
                return;
            currentDelay = attackDelay;
            flyAwayDirection = Random.insideUnitCircle.normalized;

            Shoot();
        }

        private void Shoot()
        {
            var direction = targetTransform.position - transform.position;
            var bullet = Instantiate(this.bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)))
                .GetComponent<Bullet>();

            bullet.SetDamage(damage);
            bullet.GetComponent<Rigidbody2D>().AddForce(direction.normalized * bulletSpeed);
        }
        

        private void FlyAway()
        {
            navMeshAgent.SetDestination( (Vector2) targetTransform.position + flyAwayDirection * attackRange);
        }
        
        public bool CanAttack() => currentDelay < 0;
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
