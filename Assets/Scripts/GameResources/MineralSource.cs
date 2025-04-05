using UnityEngine;

namespace GameResources
{
    public class MineralSource: MonoBehaviour
    {
        [field: SerializeField]private int Durability { get; set; } = 1;
        [field: SerializeField] private int MineralsCount { get; set; } = 1;
        [field: SerializeField] private Mineral[] MineralPrefabs { get; set; }

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
            for (int i = 0; i < MineralsCount; i++)
            {
                Instantiate(MineralPrefabs[Random.Range(0, MineralPrefabs.Length)], GetRandomPosition(transform.position), transform.rotation);   
            }
            Destroy(gameObject);
        }

        private Vector2 GetRandomPosition(Vector2 startPosition)
        {
            return new Vector2(startPosition.x + Random.Range(-5, 5), startPosition.y + Random.Range(-5, 5));  
        }
    }
}