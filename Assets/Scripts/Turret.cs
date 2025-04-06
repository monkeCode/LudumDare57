using System.Collections;
using System.Linq;
using Core;
using GameResources;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;


[RequireComponent(typeof(AudioSource))]
public class Turret : MonoBehaviour, IInteractable
{
    [Header("Gun References")]
    [SerializeField] private Transform _rightGun;
    [SerializeField] private Transform _leftGun;
    [SerializeField] private GameObject _projectilePrefab;

    [FormerlySerializedAs("_rotationSpeed")]
    [Header("Combat Settings")]
    [SerializeField] private float _baseRotationSpeed = 5f;
    [SerializeField] private float _rotationSpeedMultiplier = 1f; 
    private float RotationSpeed => _baseRotationSpeed * _rotationSpeedMultiplier;
    
    [SerializeField] private float _baseShootFreq = 1f;
    [SerializeField] private float _shootFreqMultiplier = 1f;
    private float ShootFreq => _baseShootFreq * _shootFreqMultiplier;
    
    [SerializeField] private uint _baseDamage = 10;
    [SerializeField] private float _damageMultiplier = 1f;
    private uint Damage => (uint)(_baseDamage * _damageMultiplier);
    
    [SerializeField] [Range(0,100)] private float _baseRadius = 10f;
    [SerializeField] private float _radiusMultiplier = 1f;
    private float Radius => _baseRadius * _radiusMultiplier;
    
    [SerializeField] private float _baseProjectileSpeed = 10f;
    [SerializeField] private float _projectileSpeedMultiplier = 1f;
    private float ProjectileSpeed => _baseProjectileSpeed * _projectileSpeedMultiplier;

    [Header("Targeting Settings")]
    [SerializeField] private LayerMask _entityLayer;
    [SerializeField] private float _targetRefreshRate = 0.5f;

    [SerializeField] private AudioClip _shootSound;

    private Transform _target;
    private Coroutine _shootingCoroutine;
    private Coroutine _targetingCoroutine;
    private CurrencyStorage _currencyStorage;
    private AudioSource _audio;
    public bool TryUpgrade(TurretUpgradeRequest upgradeRequest)
    {
        if(!_currencyStorage.TrySpendCurrency(upgradeRequest.Price))
            return false;

        _rotationSpeedMultiplier += upgradeRequest.RotationSpeedChangeInPercentage / 100;
        _shootFreqMultiplier += upgradeRequest.ShootFreqChangeInPercentage / 100;
        _damageMultiplier += upgradeRequest.DamageChangeInPercentage / 100;
        _radiusMultiplier += upgradeRequest.RadiusChangeInPercentage / 100;
        _projectileSpeedMultiplier += upgradeRequest.ProjectileSpeedChangeInPercentage / 100;
        return true;
    }

    public void Interact()
    {
        var upgradeRequest = GetNextUpgrade();
        if (TryUpgrade(upgradeRequest))
        {
            Debug.Log("Upgrade successful");
            return;
        }
        Debug.Log("Upgrade failed");
    }

    private TurretUpgradeRequest GetNextUpgrade()
    {
        return new TurretUpgradeRequest
        {
            Price = 10,
            DamageChangeInPercentage = 10,
            ProjectileSpeedChangeInPercentage = 10,
            RadiusChangeInPercentage = 10,
            RotationSpeedChangeInPercentage = 10,
            ShootFreqChangeInPercentage = 10
        };
    }

    private void SearchTarget()
    {
        var targets = Physics2D.OverlapCircleAll(transform.position, Radius, _entityLayer);
        if(targets.Length == 0)
        {
            _target = null;
            return;
        }

        _target = targets
            .OrderBy(it => Vector2.Distance(it.transform.position, transform.position))
            .First().transform;
    }

    private float GetAngle()
    {
        if (_target == null) return 0;
        
        var dir = (Vector2)_target.position - (Vector2)transform.position;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    private void Rotate(float angle)
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            targetRotation, 
            RotationSpeed * Time.deltaTime);
    }

    void Shoot(Vector2 pos)
    {
        if (_target == null || _projectilePrefab == null) return;

        Vector2 direction = ((Vector2)_target.position - pos).normalized;
        var projectile = Instantiate(
            _projectilePrefab, 
            pos, 
            Quaternion.identity);

        var rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * ProjectileSpeed;
        }

        var projectileScript = projectile.GetComponent<Bullet>();
        if (projectileScript != null)
        {
            projectileScript.SetDamage(Damage);
        }

        _audio.PlayOneShot(_shootSound);
    }

    IEnumerator ShootCoroutine()
    {
        while(gameObject.activeSelf)
        {
            if (_target != null)
            {
                yield return new WaitForSeconds(1f / ShootFreq / 2);
                Shoot(_rightGun.position);
                yield return new WaitForSeconds(1f / ShootFreq / 2);
                Shoot(_leftGun.position);
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator TargetSearchCoroutine()
    {
        while(gameObject.activeSelf)
        {
            SearchTarget();
            yield return new WaitForSeconds(_targetRefreshRate);
        }
    }

    void Start()
    {
        _shootingCoroutine = StartCoroutine(ShootCoroutine());
        _targetingCoroutine = StartCoroutine(TargetSearchCoroutine());
        _currencyStorage = FindFirstObjectByType<CurrencyStorage>();
        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(_target == null) return;

        // Check if target moved out of range
        if (Vector2.Distance(_target.position, transform.position) > Radius)
        {
            _target = null;
            return;
        }

        Rotate(GetAngle());
    }

    void OnDisable()
    {
        if (_shootingCoroutine != null) StopCoroutine(_shootingCoroutine);
        if (_targetingCoroutine != null) StopCoroutine(_targetingCoroutine);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
        
        if (_target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _target.position);
        }
    }
}