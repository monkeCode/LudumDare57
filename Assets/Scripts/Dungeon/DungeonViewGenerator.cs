using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dungeon
{
    public class DungeonViewGenerator : MonoBehaviour
    {
        [Header("Tilemap")]
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Tile[] tiles;
        
        [Header("Generator")]
        [SerializeField] private DungeonProvider dungeonProvider;
        [SerializeField] private bool regenerate;

        [Header("Equipment")] 
        [SerializeField] private GameObject mineral;
        [SerializeField] private GameObject weapon;
        
        private readonly List<GameObject> equipment = new();
        
        public void Start()
        {
            GenerateDungeon();
        }

        public void Update()
        {
            if (regenerate)
            {
                GenerateDungeon();
                regenerate = false;
            }
        }

        private void GenerateDungeon()
        {
            tilemap.ClearAllTiles();
            foreach (var o in equipment) 
                Destroy(o);
            
            var result = dungeonProvider.GenerateCave();

            for (var x = 0; x < result.GetLength(0); x++)
            {
                for (var y = 0; y < result.GetLength(1); y++)
                {
                    var tilePosition = new Vector3Int(x, y, 0); 
                    tilemap.SetTile(tilePosition, tiles[(int)result[x, y]]);
                }
            }

            foreach (var mineralSpawnPoint in dungeonProvider.GetMineralSpawnPoints())
                equipment.Add(Instantiate(mineral, mineralSpawnPoint, Quaternion.identity));

            foreach (var weaponSpawnPoint in dungeonProvider.GetWeaponSpawnPoints()) 
                equipment.Add(Instantiate(weapon, weaponSpawnPoint, Quaternion.identity));
        }
    }
}