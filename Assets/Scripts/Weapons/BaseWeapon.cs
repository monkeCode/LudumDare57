
using System.Collections;
using Interfaces;
using UnityEngine;

namespace Weapons
{

    public enum ShootMode
    {
        Single,
        Auto,
        Range,
        Charge,
        Shotgun

    }

    [CreateAssetMenu(menuName = "Weapons/Base")]
    public class BaseWeapon : ScriptableObject, IWeapon
    {
        [SerializeField] protected float _bulletSpeed;
        [SerializeField] protected float _shootFreq;

        [SerializeField] protected uint _damage;

        [SerializeField] protected GameObject _bullet;

        [SerializeField] protected Sprite _sprite;

        [SerializeField] protected uint _magazineSize;
        protected uint _currentAmmo;
        [SerializeField] protected float _reloadTime;

        [SerializeField] protected ShootMode _shootMode;

        [SerializeField] protected AudioClip _shootClip;
        [SerializeField] protected AudioClip _reloadClip;

        public ShootMode ShootMode => _shootMode;

        public float ReloadTime => _reloadTime;

        public float BulletSpeed => _bulletSpeed;
        public float ShootFreq => _shootFreq;

        public uint Damage => _damage;

        public uint MagazineSize => _magazineSize;

        public uint CurrentAmmo => _currentAmmo;

        public GameObject Bullet => _bullet;

        public Sprite Sprite => _sprite;

        protected bool _isReloading = false;

        public bool IsReloading => _isReloading;

        [SerializeField] protected float _spreadAngle = 2f;

        public float SpreadAngle => _spreadAngle;

        public bool Shooted { get; protected set; }

        protected float _lastShotTime;

        protected virtual IEnumerator ReloadCorutine()
        {
            yield return new WaitForSeconds(ReloadTime);
            _currentAmmo = _magazineSize;
            _isReloading = false;
        }

        public virtual void Reload()
        {
            if (_isReloading)
                return;
            _isReloading = true;
            Player.Player.Instance.StartCoroutine(ReloadCorutine());
        }

        protected void Shoot(Vector2 point, Vector2 direction)
        {
            float spread = Random.Range(-_spreadAngle, _spreadAngle);
            Quaternion spreadRotation = Quaternion.Euler(0, 0, spread);
            Vector2 spreadDirection = spreadRotation * direction;

            var bullet = Instantiate(Bullet, point, Quaternion.LookRotation(Vector3.forward, spreadDirection))
                .GetComponent<Bullet>();

            bullet.SetDamage(Damage);
            bullet.GetComponent<Rigidbody2D>().AddForce(spreadDirection * BulletSpeed);

            _currentAmmo--;
            _lastShotTime = Time.time;
        }

        public bool CanShoot() => !_isReloading && _currentAmmo > 0 && Time.time - _lastShotTime > 1f / ShootFreq && !Shooted;

        public virtual void ShootPress(Vector2 point, Vector2 direction)
        {
            if (!CanShoot())
            {
                return;
            }

            Shoot(point, direction);

            if (ShootMode == ShootMode.Single)
                Shooted = true;
            Debug.Log($"Shoot! Ammo: {_currentAmmo}/{_magazineSize}");
        }

        public virtual void ShootRelease(Vector2 point, Vector2 direction)
        {
            Debug.Log("Shoot Release");
            Shooted = false;
        }

        void OnEnable()
        {
            _currentAmmo = MagazineSize;
            _lastShotTime = Time.time;
        }
    }
}