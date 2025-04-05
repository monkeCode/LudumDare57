using Interfaces;
using UnityEngine;

namespace Weapons
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour, IBullet
    {
        [SerializeField] private float _force;
        [SerializeField] private uint _damage;

        public void SetDamage(uint damage) => _damage = damage;

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