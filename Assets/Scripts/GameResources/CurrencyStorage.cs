using TMPro;
using UnityEngine;

namespace GameResources
{
    public class CurrencyStorage : MonoBehaviour
    {
        [field: SerializeField] private int CurrencyCount { get; set; }

        public GameObject moneyUIPrefab;

        private Player.Player _player;

        void Start()
        {
            _player = FindFirstObjectByType<Player.Player>();
        }

        public void AddCurrency(int amount)
        {
            if(amount == 0)
                return;
                
            CurrencyCount += amount;
            GameObject moneyUIInstance = Instantiate(moneyUIPrefab, _player.transform);
            moneyUIInstance.transform.GetChild(0).GetComponent<TextMeshPro>().text = $"+{amount}";
        }

        public int GetCount()
        {
            return CurrencyCount;
        }

        public bool TrySpendCurrency(int amount)
        {
            if (amount > CurrencyCount) return false;
            CurrencyCount -= amount;
            GameObject moneyUIInstance = Instantiate(moneyUIPrefab, _player.transform);
            moneyUIInstance.transform.GetChild(0).GetComponent<TextMeshPro>().text = $"-{amount}";
            return true;
        }
    }
}