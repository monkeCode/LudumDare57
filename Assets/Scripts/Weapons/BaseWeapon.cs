
using Interfaces;
using UnityEngine;

namespace Weapons
{
    class BaseWeapon : ScriptableObject, IWeapon
    {
        [SerializeField] private float _shootSpeed;

        [SerializeField] private uint _damage;

        [SerializeField] private GameObject _bullet;

        public void Reload()
        {
            throw new System.NotImplementedException();
        }

        public void ShootPress(Vector2 direction)
        {
            throw new System.NotImplementedException();
        }

        public void ShootRelease(Vector2 direction)
        {
            throw new System.NotImplementedException();
        }
    }
}