using UnityEngine;

namespace GameResources
{
    public class MineralSellNoInteract : MonoBehaviour
    {
        private Player.Player _player;
        private CurrencyStorage _currencyStorage;

        private void Start()
        {
            _player = FindFirstObjectByType<Player.Player>();
            _currencyStorage = FindFirstObjectByType<CurrencyStorage>();
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _currencyStorage.AddCurrency((int)_player.inventory.SellAllForMoney());
            }
        }
    }
}
