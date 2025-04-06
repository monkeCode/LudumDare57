using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

namespace Weapons
{
    class WorldWeapon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        
        [Header("amimation")]
        [SerializeField] private float amplitude = 0.2f;
        [SerializeField] private float speed = 1f;

        [Header("Weapon")]
        [SerializeField] private BaseWeapon _weapon;

        [Header("Internal")]

        [SerializeField] private SpriteRenderer _sp;
        [SerializeField] private Light2D _light;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("pointer enter");
            Player.Player.Instance.Inputs.Player.Interact.started += ctx => PlayerLoot();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("pointer exit");
            Player.Player.Instance.Inputs.Player.Interact.started -= ctx => PlayerLoot();
        }

        public void PlayerLoot()
        {
            if(Vector2.Distance(transform.position, Player.Player.Instance.transform.position) > 2)
                return;
            var weapon = Player.Player.Instance.WeaponHandler.SwapWeapon(_weapon);
            SetWeapon(weapon);
            Debug.Log("PlayerLoot");
        }

        public void SetWeapon([NotNull] BaseWeapon weapon)
        {
            _weapon = weapon;

            if (_sp != null)
                _sp.sprite = _weapon.Sprite;

            switch (_weapon.Rarity)
            {
                case Rarity.Common:
                    _light.color = Color.white;
                    break;
                case Rarity.Uncommon:
                    _light.color = Color.green;
                    break;
                case Rarity.Rare:
                    _light.color = Color.magenta;
                    break;
            }

        }

        void Start()
        {
            if (_weapon != null)
                SetWeapon(_weapon);
        }


        void Update()
        {
            
            Vector3 basePosition = transform.position;

            float newY = basePosition.y + Mathf.Sin(Time.time * speed) * amplitude;
            _sp.transform.position = new Vector3(basePosition.x, newY, basePosition.z);

            _sp.transform.Rotate(0, 0, 15 * Time.deltaTime);
        }


    [ContextMenu("Generate gun")]
    private void GenerateGun()
    {
        var weapon = WeaponGenerator.Instance.GenerateWeapon(Rarity.Rare);

        SetWeapon(weapon);
    }
}
}