using Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    
    [SerializeField] private int damage;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    public Transform target;

    Rigidbody2D _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var dir = (Vector2)(target.position - transform.position).normalized;
        var v = _rb.linearVelocity + acceleration * Time.deltaTime * dir;
        _rb.linearVelocity = Vector2.ClampMagnitude(v, maxSpeed);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage((uint)damage);
        }
        Destroy(gameObject);
    }
}
