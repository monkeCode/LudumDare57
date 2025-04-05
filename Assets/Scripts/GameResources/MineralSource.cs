using Interfaces;
using UnityEngine;

namespace GameResources
{
    public class MineralSource: MonoBehaviour, IDamageable
    {
        [field: SerializeField] private int Durability { get; set; } = 1;
        [field: SerializeField] private uint MineralsCount { get; set; } = 1;
        [field: SerializeField] private Mineral[] MineralPrefabs { get; set; }

        [field: SerializeField] private float SpawnRadius { get; set; } = 0.5f; 

        private Vector2 GetRandomPosition(Vector2 startPosition)
        {
            return new Vector2(startPosition.x + Random.Range(-SpawnRadius, SpawnRadius), startPosition.y + Random.Range(-SpawnRadius, SpawnRadius));  
        }

        public void TakeDamage(uint damage)
        {
            Durability -= (int)damage;
            Debug.Log(Durability);
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