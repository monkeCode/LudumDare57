using Core;
using UnityEngine;

namespace GameResources
{
    public class MineralSellPoint: MonoBehaviour, IInteractable
    {
        private Player.Player _player;
        private CurrencyStorage _currencyStorage;

        private void Start()
        {
            _player = FindFirstObjectByType<Player.Player>();
            _currencyStorage = FindFirstObjectByType<CurrencyStorage>();
        }

        public void Interact()
        {
            _currencyStorage.AddCurrency((int)_player.inventory.SellAllForMoney());
        }
    }
}