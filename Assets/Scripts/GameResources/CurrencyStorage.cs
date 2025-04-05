using UnityEngine;

namespace GameResources
{
    public class CurrencyStorage: MonoBehaviour
    {
        [field: SerializeField] private int CurrencyCount { get; set; }

        public void AddCurrency(int amount)
        {
            CurrencyCount += amount;
        }

        public int GetCount()
        {
            return CurrencyCount;
        }

        public bool TrySpendCurrency(int amount)
        {
            if (amount > CurrencyCount) return false;
            CurrencyCount -= amount;
            return true;
        }
    }
}