using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMover))]
    class Player:MonoBehaviour
    {
        
        private InputSystem_Actions _inputs;
        private PlayerMover _mover;

        private void Awake()
        {
            _inputs = new InputSystem_Actions();
            _inputs.Player.Enable();

            _mover = GetComponent<PlayerMover>();
            _mover.Init(_inputs);
        }

        void OnEnable()
        {
            _inputs?.Player.Enable();
        }

        void OnDisable()
        {
            _inputs?.Player.Enable();
        }

    }

}
