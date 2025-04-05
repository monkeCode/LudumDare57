using UnityEngine;

namespace Currency
{
    public class CurrencySource: MonoBehaviour
    {
        [field: SerializeField]private int Durability { get; set; } = 1;
        [field: SerializeField] private int CurrencyCount { get; set; } = 1;
        [field: SerializeField] private GameObject CurrencyItemPrefab { get; set; }

        private void OnHit()
        {
            Durability -= 1;
            if (Durability <= 0)
            {
                Destroy();
            }
        }

        private void Destroy()
        {
            for (int i = 0; i < CurrencyCount; i++)
            {
                Instantiate(CurrencyItemPrefab, GetRandomPosition(transform.position), transform.rotation);   
            }
            Destroy(gameObject);
        }

        private Vector2 GetRandomPosition(Vector2 startPosition)
        {
            return new Vector2(startPosition.x + Random.Range(-5, 5), startPosition.y + Random.Range(-5, 5));  
        }
    }
}