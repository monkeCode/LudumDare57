using System;
using System.Collections;
using System.Linq;
using Interfaces;
using UnityEngine;
using Weapons;

public class Grenade : Bullet
{
    [SerializeField] private float _boomrange;
    [SerializeField] private float _boomTimer;
    [SerializeField] private GameObject _boomPrefab;
    [SerializeField] private LayerMask _boomLayer;

    IEnumerator WaitAndBOOOOOOOOOOM()
    {
        yield return new WaitForSeconds(_boomTimer);
        BOOOOOOM();
    }

    private void BOOOOOOM()
    {
        var boomObjects = Physics2D.OverlapCircleAll(transform.position, _boomrange, _boomLayer);

        foreach (var boomTarget in boomObjects)
        {
            if (boomTarget.TryGetComponent(out IDamageable boomDamageable))
            {
                float distance = Vector2.Distance(transform.position, boomTarget.transform.position);
                var boomCoef = Math.Clamp((_boomrange - distance) / _boomrange, 0, 1); Debug.Log(boomCoef);

                boomDamageable.TakeDamage((uint)(boomCoef * _damage));
                if (boomTarget.TryGetComponent<Rigidbody2D>(out var rb))
                {
                    rb.AddForce((boomTarget.transform.position - transform.position).normalized * _force * boomCoef);
                }
            }
        }

        Instantiate(_boomPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected override void Start()
    {
        StartCoroutine(WaitAndBOOOOOOOOOOM());
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _boomrange);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_boomLayer == (_boomLayer | (1 << collision.gameObject.layer)))
        {
            StopCoroutine(WaitAndBOOOOOOOOOOM());
            BOOOOOOM();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {

    }
}
