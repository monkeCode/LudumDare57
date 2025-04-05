
using System;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;

namespace Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    class WeaponHandler : MonoBehaviour
    {

        [SerializeField] private BaseWeapon _weapon;

        private SpriteRenderer _spriteRenderer;

        private bool press = false;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_weapon != null)
            {
                SwapWeapon(_weapon);
            }
        }

        public void UpdateRotation(Vector2 trackPoint)
        {
            var dir = trackPoint - (Vector2)transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            _spriteRenderer.flipY = dir.x < 0;
            Debug.DrawRay(transform.position, dir);
        }

        public BaseWeapon SwapWeapon(BaseWeapon newWeapon)
        {
            var curWeapon = _weapon;

            _weapon = newWeapon;
            _spriteRenderer.sprite = _weapon.Sprite;

            return curWeapon;
        }

        public void ShootPress()
        {
            press = true;
        }

        public void ShootRelease()
        {
            press = false;
            _weapon.ShootRelease(transform.position, transform.right);
        }

        public void Reload()
        {
            _weapon.Reload();
        }

        private void Update()
        {
            if(press)
                _weapon.ShootPress(transform.position, transform.right);
        }
    }
}