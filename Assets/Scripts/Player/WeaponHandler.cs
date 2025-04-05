
using Unity.VisualScripting;
using UnityEngine;
using Weapons;

namespace Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    class WeaponHandler:MonoBehaviour
    {
        
        [SerializeField] private BaseWeapon _weapon;

        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if(_weapon != null)
            {
                SwapWeapon(_weapon);
            }
        }

        public void UpdateRotation(Vector2 trackPoint)
        {
            var dir = trackPoint - (Vector2)transform.position;
            var angle = Vector2.Angle(Vector2.up, dir);

            transform.rotation = Quaternion.Euler(new Vector3(0,0, angle));
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
            _weapon.ShootPress(transform.right);
        }

        public void ShootRelease()
        {
            _weapon.ShootRelease(transform.right);
        }

        public void Reload()
        {
            _weapon.Reload();
        }
    }
}