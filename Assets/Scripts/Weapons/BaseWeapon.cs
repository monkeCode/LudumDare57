
using Interfaces;
using UnityEngine;

namespace Weapons
{
    class BaseWeapon : ScriptableObject, IWeapon
    {
        [SerializeField] private float _shootSpeed;

        [SerializeField] private uint _damage;

        [SerializeField] private GameObject _bullet;

        [SerializeField] private Sprite _sprite;

        public float ShootSpeed => _shootSpeed;

        public uint Damage => _damage;

        public GameObject Bullet => _bullet;

        public Sprite Sprite => _sprite;

        public void Reload()
        {
            Debug.Log("Reload");
        }

        public void ShootPress(Vector2 direction)
        {
            Debug.Log("Shoot Press");
        }

        public void ShootRelease(Vector2 direction)
        {
            Debug.Log("Shoot Release");
        }
    }
}