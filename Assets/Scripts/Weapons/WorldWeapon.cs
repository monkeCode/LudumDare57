using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TMPro;
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
        
        [SerializeField] private Canvas _cv;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private float _showAfterSecs;

        private bool _show = false;

        IEnumerator Waiter()
        {
            yield return new WaitForSeconds(_showAfterSecs);
            if(!_show)
                yield break;

            _cv.gameObject.SetActive(true);
            _text.text = GetText();
            var color = _weapon.Rarity switch
            {
                Rarity.Common => "white",
                Rarity.Uncommon => "green",
                Rarity.Rare => "purple",
                _ => throw new System.NotImplementedException()
            };
            _name.text = $"<color={color}>{_weapon.Name}</color>";
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("pointer enter");
            Player.Player.Instance.Inputs.Player.Interact.started += ctx => PlayerLoot();
            if(_show)
                return;
            _show = true;
            StartCoroutine(Waiter());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("pointer exit");
            Player.Player.Instance.Inputs.Player.Interact.started -= ctx => PlayerLoot();
            _cv.gameObject.SetActive(false);
            StopCoroutine(Waiter());
            _show = false;
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

            _cv.gameObject.SetActive(false);
            StopCoroutine(Waiter());
            _show = false;

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


    private string GetText()
    {
        StringBuilder s= new StringBuilder();
        if(_weapon is Shotgun sg)
            s.Append($"<b>Damage</b>: {_weapon.Damage}x{sg.BulletsCount}\n");
        else
            s.Append($"<b>Damage</b>: {_weapon.Damage}\n");
        s.Append($"<b>Rate</b>: {Math.Round(_weapon.ShootFreq, 2)}\n");
        s.Append($"<b>Reload Time</b>: {Math.Round(_weapon.ReloadTime, 2)}\n");
        s.Append($"<b>Ammo</b>: {_weapon.MagazineSize}\n");

        return s.ToString();
    }
}
}