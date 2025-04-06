
using System;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;

namespace Player
{
    [RequireComponent(typeof(SpriteRenderer), typeof(AudioSource))]
    class WeaponHandler : MonoBehaviour
    {

        [SerializeField] private BaseWeapon _weapon;
        [SerializeField] private float _offset;
        [SerializeField] private float _time;
        [SerializeField] private Transform _center;

        private Vector2 trackPoint;
        private SpriteRenderer _spriteRenderer;

        private AudioSource source;

        private bool press = false;

        public bool IsReloading => _weapon.IsReloading;

        public BaseWeapon Weapon => _weapon;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            source = GetComponent<AudioSource>();

            if (_weapon != null)
            {
                SwapWeapon(_weapon);
            }
        }

        public void UpdatePosition(Vector2 trackPoint)
        {
            var dir = trackPoint - (Vector2)_center.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            _spriteRenderer.flipY = dir.x < 0;
            this.trackPoint = (Vector2)_center.position + dir.normalized * _offset;

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

        public void Drift()
        {
            transform.position += -(Vector3)(trackPoint-(Vector2)_center.position) * 0.1f + Vector3.up * 0.1f;
        }

        public void PlaySound(AudioClip clip)
        {
            source.PlayOneShot(clip);
        }

        private void Update()
        {
            if(press)
                _weapon.ShootPress(transform.position, transform.right);

            transform.position =  Vector2.Lerp(transform.position, trackPoint, _time * Time.deltaTime);
        }

    }
}