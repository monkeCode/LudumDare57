using Enemies;
using Interfaces;
using UnityEngine;

public class Boss : MonoBehaviour, IEnemy
{
    enum BossState
    {
        Idle,
        Atk,
        Ability1,
        Ability2,
        Ability3
    }

    [Header("Stats")]
    [field: SerializeField] public int Hp { get; private set; }
    [field: SerializeField] public int MaxHp { get; private set; }
    [field: SerializeField] public float speed { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float DamageRadius { get; private set; }
    [field: SerializeField] public float TargetRadius { get; private set; }

    private Transform _target;
    private Animator _animator;

    [Header("Teleport")]
    [SerializeField] public float _tpProbability;
    [SerializeField] public float TeleportRange;
    [SerializeField] private float _teleportCooldown;
    private float _lastTeleportTime;

    [Header("Balls")]
    [SerializeField] public float _ballsProbability;
    [SerializeField] public float _ballsCooldown;
    [SerializeField] public int BallsMinCount;
    [SerializeField] public int BallsMaxCount;

    private float _lastBallTime;
    public int BallSCount => Random.Range(BallsMinCount, BallsMaxCount);
    [SerializeField] Ball BallPrefab;

    [Header("Change Target")]
    [SerializeField] public float _burstProb;

    private bool _abilityPerformed = false;
    private BossState _state;
    private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _sp;

    [SerializeField] private LayerMask _damageMask;

    private Transform _playerTransform;
    private Transform _platformTransform;

    private bool _abilityStarted;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerTransform = Player.Player.Instance.transform;
        _platformTransform = Platform.Instance.transform;
        _state = BossState.Idle;
        Hp = MaxHp;
        _lastTeleportTime = Time.time;
        _lastBallTime = Time.time;
    }

    void MoveTo(Vector2 point)
    {
        Vector2 direction = (point - (Vector2)transform.position).normalized;
        _rb.MovePosition(_rb.position + direction * speed * Time.deltaTime);

        if (direction.x != 0)
        {
            _sp.flipX = direction.x < 0;
        }

        _animator.SetBool("Move", true);
    }

    void Atk()
    {
        float distance = Vector2.Distance(transform.position, _target.position);
        if(distance > DamageRadius * 3)
        {
            _abilityPerformed = true;
        }
        if (distance > DamageRadius)
        {
            MoveTo(_target.position);
        }
        else if(!_abilityStarted)
        {
            _abilityStarted = true;
            _animator.SetTrigger("Attack");
            _state = BossState.Idle;
        }
    }

    public void AnimAtk()
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, DamageRadius, _damageMask);
        foreach (var target in hitTargets)
        {
            target.GetComponent<IDamageable>().TakeDamage((uint)Damage);
        }
        _abilityPerformed = true;
    }

    public void AnimTeleport()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 teleportPosition = (Vector2)_playerTransform.position + randomDirection * TeleportRange;
        int i = 0 ;
        while (Physics2D.OverlapCircle(teleportPosition, 1f, LayerMask.GetMask("Ground")) != null && i < 100)
        {
            randomDirection = Random.insideUnitCircle.normalized;
            teleportPosition = (Vector2)_playerTransform.position + randomDirection * TeleportRange;
            i++;
        }
        _target = _platformTransform;
        transform.position = teleportPosition;
        _abilityPerformed = true;
    }

    void Teleport()
    {
        if (Time.time > (_lastTeleportTime + _teleportCooldown) && Vector2.Distance(_platformTransform.position, transform.position) > TargetRadius/2 && !_abilityStarted)
        {
            _abilityStarted = true;
            _animator.SetTrigger("Teleport");
            _lastTeleportTime = Time.time;
        }
        else if (!_abilityStarted)
        {
            _abilityPerformed = true;
        }
    }

    void Balls()
    {
        if (Time.time > (_lastBallTime + _ballsCooldown) && !_abilityStarted)
        {
            _abilityStarted = true;
            _animator.SetTrigger("Summon");
            _lastBallTime = Time.time;
        }
        else if(!_abilityStarted)
        _abilityPerformed = true;
    }

    public void SpawnBallsAnim()
    {
        int count = BallSCount;
        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPos = transform.position + (Vector3)Random.insideUnitCircle * 4f;
            Instantiate(BallPrefab, spawnPos, Quaternion.identity).target = _target;
        }
        _abilityPerformed = true;
    }



    void Burst()
    {
        if(_target == _platformTransform)
            _target =_playerTransform;
        else if(_target == _playerTransform)
            _target = _platformTransform;
        _abilityPerformed = true;
    }


    public void Heal(uint heals)
    {
        Hp = Mathf.Min(Hp + (int)heals, MaxHp);
    }

    public void Kill()
    {
        Die();
    }

    void Die()
    {
        _animator.SetTrigger("Die");
        Destroy(gameObject, 3f);
    }

    public void TakeDamage(uint damage)
    {
        Hp -= (int)damage;
        if (Hp <= 0)
        {
            Die();
        }
    }

    void SearchTarget()
    {
        if (Vector2.Distance(transform.position, _playerTransform.position) < Vector2.Distance(transform.position, _platformTransform.position))
        {
            _target = _playerTransform;
        }
        else
            _target = _platformTransform;
    }

    void ChooseAction()
    {
        var dist = Vector2.Distance(_target.position, transform.position);

        if(dist < DamageRadius)
        {
            _state = BossState.Atk;
        }

        float random = Random.value;

        if (random < _tpProbability)
        {
            _state = BossState.Ability1;
        }
        else if (random < (_tpProbability + _ballsProbability) &&  dist < TargetRadius)
        {
            _state = BossState.Ability2;
        }
        else if (random < (_tpProbability + _ballsProbability + _burstProb))
        {
            _state = BossState.Ability3;
        }
        else
        {
            _state = BossState.Atk;
        }
    }

    void Update()
    {
        Debug.Log(_state);
        Debug.Log(_target);
        if (_target == null || Vector2.Distance(transform.position, _target.position) > TargetRadius)
        {
            SearchTarget();
        }

        if (_state == BossState.Idle)
        {
            ChooseAction();
        }
        else if (_state == BossState.Atk)
        {
            Atk();
        }
        else if (_state == BossState.Ability1)
        {
            Teleport();
        }
        else if (_state == BossState.Ability2)
        {
            Balls();
        }
        else if (_state == BossState.Ability3)
        {
            Burst();
        }

        if (_abilityPerformed)
        {
            _state = BossState.Idle;
            _abilityPerformed = false;
            _abilityStarted = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DamageRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, TargetRadius);
        Gizmos.color = Color.blue;
    }
}