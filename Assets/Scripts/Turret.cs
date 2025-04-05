using System.Collections;
using System.Linq;
using UnityEngine;
using Weapons;


public class Turret : MonoBehaviour
{
    [Header("Gun References")]
    [SerializeField] private Transform _rightGun;
    [SerializeField] private Transform _leftGun;
    [SerializeField] private GameObject _projectilePrefab;

    [Header("Combat Settings")]
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _shootFreq = 1f;
    [SerializeField] private uint _damage = 10;
    [SerializeField] [Range(0,100)] private float _radius = 10f;
    [SerializeField] private float _projectileSpeed = 10f;

    [Header("Targeting Settings")]
    [SerializeField] private LayerMask _entityLayer;
    [SerializeField] private float _targetRefreshRate = 0.5f;

    private Transform _target;
    private Coroutine _shootingCoroutine;
    private Coroutine _targetingCoroutine;

    private void SearchTarget()
    {
        var targets = Physics2D.OverlapCircleAll(transform.position, _radius, _entityLayer);
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
            _rotationSpeed * Time.deltaTime);
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
            rb.linearVelocity = direction * _projectileSpeed;
        }

        var projectileScript = projectile.GetComponent<Bullet>();
        if (projectileScript != null)
        {
            projectileScript.SetDamage(_damage);
        }
    }

    IEnumerator ShootCoroutine()
    {
        while(gameObject.activeSelf)
        {
            if (_target != null)
            {
                yield return new WaitForSeconds(1f / _shootFreq / 2);
                Shoot(_rightGun.position);
                yield return new WaitForSeconds(1f / _shootFreq / 2);
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
    }

    void Update()
    {
        if(_target == null) return;

        // Check if target moved out of range
        if (Vector2.Distance(_target.position, transform.position) > _radius)
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
        Gizmos.DrawWireSphere(transform.position, _radius);
        
        if (_target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _target.position);
        }
    }
}