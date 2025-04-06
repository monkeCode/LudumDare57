using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemies
{
    public class Flier : MonoBehaviour, IEnemy
    {
        [SerializeField] private uint health = 50;
        [SerializeField] private uint damage = 5;
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private float engageRange = 10f;
        [SerializeField] private float attackDelay = 3f;
        [SerializeField] private float engageDelay = 1f;
        private float lastAttackTime;
        private bool engaging;
        
        [SerializeField] private float speed = 5f;
        [SerializeField] private float engageSpeed = 15f;

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
            if (NearTarget(engageRange) && !engaging)
            {
                engaging = true;
                StartCoroutine(PerformEngage());
            }
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
            lastAttackTime = Time.time;
            targetDamageable.TakeDamage(damage);
            engaging = false;
        }
        
        IEnumerator PerformEngage()
        {
            var directionToTarget = transform.position - targetTransform.position;
            flyAwayDirection = directionToTarget * -1;

            navMeshAgent.speed = 0f;
            
            yield return new WaitForSeconds(engageDelay);
            
            navMeshAgent.speed = engageSpeed;
        }

        private void FlyAway()
        {
            navMeshAgent.speed = speed;
            var normalized = flyAwayDirection.normalized * 10;
            navMeshAgent.SetDestination(normalized);
        }
        
        public bool CanAttack() => Time.time - lastAttackTime > attackDelay || Time.time < attackDelay;
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
