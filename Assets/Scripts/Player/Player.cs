using System;
using GameResources;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerMover))]
    class Player : MonoBehaviour, IDamageable, IStats
    {

        [Header("Stats")]
        [SerializeField] private uint _hp;
        [SerializeField] private uint _maxHp;

        [Header("Trash")]
        [SerializeField] WeaponHandler _weaponHandler;

        private InputSystem_Actions _inputs;
        private PlayerMover _mover;

        public uint MaxHp { get => _maxHp; set => _maxHp = value; }
        public uint Hp { get => _hp; set => _hp = Math.Clamp(value, 0, _maxHp); }

        public static Player Instance { get; private set; }

        public readonly PlayerInventory inventory = new();

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

            _mover = GetComponent<PlayerMover>();
            _inputs.Player.Jump.started += ctx => _mover.Jump();
            _inputs.Player.Jump.canceled += ctx => _mover.CutJump();
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
            Hp -= damage;
            if (Hp == 0)
                Die();
        }

        private void Die()
        {
            throw new NotImplementedException();
        }

        public void Kill()
        {
            Die();
        }

        public void Heal(uint heals)
        {
            Hp += heals;
        }

        private void Update()
        {   
            _mover.Move(_inputs.Player.Move.ReadValue<Vector2>().x, SpeedModifier);
            _weaponHandler.UpdateRotation(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        }
    }

}
