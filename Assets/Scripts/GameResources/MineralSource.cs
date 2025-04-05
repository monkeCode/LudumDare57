using Interfaces;
using UnityEngine;

namespace GameResources
{
    public class MineralSource: MonoBehaviour, IDamageable
    {
        [field: SerializeField] private uint Durability { get; set; } = 1;
        [field: SerializeField] private uint MineralsCount { get; set; } = 1;
        [field: SerializeField] private Mineral[] MineralPrefabs { get; set; }

        private Vector2 GetRandomPosition(Vector2 startPosition)
        {
            return new Vector2(startPosition.x + Random.Range(-5, 5), startPosition.y + Random.Range(-5, 5));  
        }

        public void TakeDamage(uint damage)
        {
            Durability -= damage;
            if (Durability <= 0)
            {
                Kill();
            }
        }

        // Can't heal
        public void Heal(uint heals) { }

        public void Kill()
        {
            for (int i = 0; i < MineralsCount; i++)
            {
                Instantiate(MineralPrefabs[Random.Range(0, MineralPrefabs.Length)], GetRandomPosition(transform.position), transform.rotation);   
            }
            Destroy(gameObject);
        }
    }
}