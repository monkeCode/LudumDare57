
using System.Collections;
using Interfaces;
using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(menuName ="Weapons/TestWeapon")]
    class BaseWeapon : ScriptableObject, IWeapon
    {
        [SerializeField] protected float _shootSpeed;

        [SerializeField] protected uint _damage;

        [SerializeField] protected GameObject _bullet;

        [SerializeField] protected Sprite _sprite;

        [SerializeField] protected uint _magazineSize;
        [SerializeField] protected uint _currentAmmo;
        [SerializeField] protected float _reloadTime;

        public float ReloadTime => _reloadTime;

        public float ShootSpeed => _shootSpeed;

        public uint Damage => _damage;

        public uint MagazineSize => _magazineSize;

        public uint CurrentAmmo => _currentAmmo;

        public GameObject Bullet => _bullet;

        public Sprite Sprite => _sprite;

        protected bool _isReloading = false;

        public bool IsReloading => _isReloading;

        [SerializeField] protected float _spreadAngle = 2f;

        public float SpreadAngle => _spreadAngle;

        protected float _lastShotTime;

        protected virtual IEnumerator ReloadCorutine()
        {
            yield return new WaitForSeconds(ReloadTime);
            _currentAmmo = _magazineSize;
            _isReloading = false;
        }

        public virtual void Reload()
        {
            if(_isReloading)
                return;
            _isReloading = true;
            Player.Player.Instance.StartCoroutine(ReloadCorutine());
        }

        protected void Shoot(Vector2 point, Vector2 direction)
        {
            if (_isReloading || _currentAmmo <= 0)
            {
                return;
            }

            if (Time.time - _lastShotTime >= 1f / ShootSpeed)
            {
                float spread = Random.Range(-_spreadAngle, _spreadAngle);
                Quaternion spreadRotation = Quaternion.Euler(0, 0, spread);
                Vector2 spreadDirection = spreadRotation * direction;

                var bullet = Instantiate(Bullet, point, Quaternion.LookRotation(Vector3.forward, spreadDirection))
                    .GetComponent<Bullet>();
                
                bullet.SetDamage(Damage);
                bullet.GetComponent<Rigidbody2D>().AddForce(spreadDirection * ShootSpeed);

                _currentAmmo--;
                _lastShotTime = Time.time;
            }
        }

        public virtual void ShootPress(Vector2 point, Vector2 direction)
        {
            Shoot(point, direction);
            Debug.Log($"Shoot! Ammo: {_currentAmmo}/{_magazineSize}");
        }

        public virtual void ShootRelease(Vector2 point, Vector2 direction)
        {
            Debug.Log("Shoot Release");
        }
    }
}