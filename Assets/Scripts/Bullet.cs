using Interfaces;
using UnityEngine;

namespace Weapons
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour, IBullet
    {
        [SerializeField] protected float _force;
        [SerializeField] protected uint _damage;

        public void SetDamage(uint damage) => _damage = damage;

        protected virtual void Start()
        {
            Destroy(gameObject, 10);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_damage);
                if (other.TryGetComponent<Rigidbody2D>(out var rb))
                {
                    rb.AddForce((other.transform.position - transform.position).normalized * _force);
                }
                Destroy(gameObject);
            }
        }
    }
}