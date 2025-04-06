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
    [SerializeField] private AudioClip _boomSound;
    [SerializeField] private GameObject _boomPrefab;
    [SerializeField] private LayerMask _boomLayer;

    IEnumerator WaitAndBOOOOOOOOOOM()
    {
        yield return new WaitForSeconds(_boomTimer);

        var boomObjects = Physics2D.OverlapCircleAll(transform.position, _boomrange, _boomLayer);

        foreach(var boomTarget in boomObjects)
        {
            if(boomTarget.TryGetComponent(out IDamageable boomDamageable))
            {
                var boomCoef = 1-MathF.Pow(Vector2.Distance(transform.position, boomTarget.transform.position), 2)/_boomrange;
                boomDamageable.TakeDamage((uint)( boomCoef * _damage));
                if (boomTarget.TryGetComponent<Rigidbody2D>(out var rb))
                {
                    rb.AddForce((boomTarget.transform.position - transform.position).normalized * _force * boomCoef);
                }
            }
        }

        Instantiate(_boomPrefab);
        Destroy(gameObject);

    }
    protected override void Start()
    {
        StartCoroutine(WaitAndBOOOOOOOOOOM());
    }
}
