using TMPro;
using UnityEngine;
using Weapons;

namespace UI.GameplayUI
{
    public class Ammo: MonoBehaviour
    {
        private Player.Player _player;
        [SerializeField] private TextMeshProUGUI ammoText;
        private BaseWeapon currentWeapon;
        
        private void Start()
        {
            _player = FindFirstObjectByType<Player.Player>();
            currentWeapon = _player.WeaponHandler.weapon;
            AddHandlers(currentWeapon);
            _player.WeaponHandler.OnSwapWeapon.AddListener(OnSwapWeaponHandler);
        }

        private void OnShootHandler()
        {
            Debug.Log("On shoot handler");
            UpdateText();
        }

        private void OnReloadedHandler()
        {
            UpdateText();
        }

        private void OnSwapWeaponHandler()
        { 
            RemoveHandlers(currentWeapon);
            currentWeapon = _player.WeaponHandler.weapon;
            AddHandlers(currentWeapon);
            UpdateText();
        }

        private void RemoveHandlers(BaseWeapon baseWeapon)
        {
            baseWeapon.OnShoot.RemoveListener(OnShootHandler);
            baseWeapon.OnReloaded.RemoveListener(OnReloadedHandler);
        }

        private void AddHandlers(BaseWeapon baseWeapon)
        {
            baseWeapon.OnShoot.AddListener(OnShootHandler);
            baseWeapon.OnReloaded.AddListener(OnReloadedHandler);
        }

        private void UpdateText()
        {
            ammoText.text = $"{_player.WeaponHandler.weapon.CurrentAmmo}\\{_player.WeaponHandler.weapon.MagazineSize}";
        }
    }
}