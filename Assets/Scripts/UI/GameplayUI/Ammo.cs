using TMPro;
using UnityEngine;

namespace UI.GameplayUI
{
    public class Ammo: MonoBehaviour
    {
        private Player.Player _player;
        [SerializeField] private TextMeshProUGUI ammoText;
        
        private void Start()
        {
            _player = FindFirstObjectByType<Player.Player>();
            _player.WeaponHandler.weapon.OnShoot.AddListener(OnShootHandler);
            _player.WeaponHandler.weapon.OnReloaded.AddListener(OnReloadedHandler);
            _player.WeaponHandler.OnSwapWeapon.AddListener(OnSwapWeaponHandler);
        }

        private void OnShootHandler()
        {
            UpdateText();
        }

        private void OnReloadedHandler()
        {
            UpdateText();
        }

        private void OnSwapWeaponHandler()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            ammoText.text = $"{_player.WeaponHandler.weapon.CurrentAmmo}\\{_player.WeaponHandler.weapon.MagazineSize}";
        }
    }
}