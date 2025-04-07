using System;
using GameResources;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerMover))]
    class Player : PausedBehavour, IDamageable, IStats
    {

        [Header("Stats")]
        [SerializeField] private int _hp;
        [SerializeField] private int _maxHp;

        [Header("Trash")]
        [SerializeField] WeaponHandler _weaponHandler;

        private Animator _animator;

        public WeaponHandler WeaponHandler => _weaponHandler;
        public InputSystem_Actions Inputs => _inputs;
        private InputSystem_Actions _inputs;
        private PlayerMover _mover;
        private Platform _platform;
        private GameObject _respawnPoint;
        
        private AudioSource source;
        [SerializeField] private AudioClip _deathSound;

        public int MaxHp { get => _maxHp; set => _maxHp = value; }
        public int Hp { get => _hp; set => _hp = Math.Clamp(value, 0, _maxHp); }

        public static Player Instance { get; private set; }

        public readonly PlayerInventory inventory = new();
        
        [field: SerializeField] public Mineral MineralPrefab { get; set; }

        private const float MinSpeedModifier = 0.2f;
        public float SpeedModifier => Math.Max(MinSpeedModifier, 1 - inventory.CurrentWeight / inventory.MaxWeight);
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }

            _inputs = new InputSystem_Actions();
            _inputs.Player.Enable();

            _animator = GetComponent<Animator>();

            _mover = GetComponent<PlayerMover>();
            _inputs.Player.Jump.started += ctx => _mover.Jump();
            _inputs.Player.Jump.started += ctx => _animator.SetTrigger("jmp");
            _inputs.Player.Jump.canceled += ctx => _mover.CutJump();

            _inputs.Player.Attack.started += ctx => _weaponHandler.ShootPress();
            _inputs.Player.Attack.canceled += ctx => _weaponHandler.ShootRelease();
            _inputs.Player.Reload.performed += ctx => _weaponHandler.Reload();

            _platform = FindFirstObjectByType<Platform>();
            _respawnPoint = _platform.PlatformRespawnPoint;
            source = GetComponent<AudioSource>();
        }

        void OnEnable()
        {
            _inputs?.Player.Enable();
        }

        void OnDisable()
        {
            _inputs?.Player.Enable();
        }

        public void TakeDamage(uint damage)
        {
            Hp -= (int)damage;
            if (Hp == 0)
                Die();
        }

        private void Die()
        {
            ResetHealth();
            Respawn();
            _platform.TakeDamage((uint)_platform.maxHealth/10);
        }

        public void Kill()
        {
            Die();
        }

        public void Heal(uint heals)
        {
            Hp += (int)heals;
        }

        public void DropMineral()
        {
            var mineral = inventory.Pop();
            var instance = Instantiate(MineralPrefab, gameObject.transform.position, gameObject.transform.rotation);
            instance.Cost = mineral.Cost;
            instance.Size = mineral.Size;
            // instance.transform.localScale = new Vector3(mineral.Size, mineral.Size, 1);
        }

        protected override void InnerUpdate()
        {   
            _mover.Move(_inputs.Player.Move.ReadValue<Vector2>().x, SpeedModifier);
            var look_dir = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            _weaponHandler.UpdatePosition(look_dir);
            HandleInputs();

            _animator.SetBool("onGround", _mover.OnGround);
            _animator.SetBool("run", _inputs.Player.Move.ReadValue<Vector2>().x != 0);
            _animator.SetFloat("dy", _mover.GetVelocity().y);
        }

        private void HandleInputs()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropMineral();
            }
        }

        private void ResetHealth()
        {
            Hp = MaxHp;
        }

        private void Respawn()
        {
            gameObject.transform.position = _respawnPoint.transform.position;
        }
    }

}
