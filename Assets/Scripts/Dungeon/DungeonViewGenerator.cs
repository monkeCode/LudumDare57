using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Weapons;

namespace Dungeon
{
    public class DungeonViewGenerator : MonoBehaviour
    {
        [Header("Tilemap")]
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Tile _wallTile;

        [Header("Generator")]
        [SerializeField] private DungeonProvider dungeonProvider;
        [SerializeField] private bool regenerate;
        [SerializeField] int _platformWidth = 6;

        [Header("Equipment")]
        [SerializeField] private GameObject mineral;
        [SerializeField] private GameObject weapon;

        public readonly List<List<GameObject>> equipment = new();


        public void Update()
        {
            if (regenerate)
            {
                GenerateDungeon();
                regenerate = false;
            }
        }

        public List<List<GameObject>> GenerateDungeon()
        {
            tilemap.ClearAllTiles();
            foreach (var o in equipment.SelectMany(i => i))
                Destroy(o);

            int offsetY = 0;
            foreach(GeneratorStage stage in GameManager.Instance.Stages)
            {
                (var result, var stageItems) = GeterateSide(new Vector2Int(_platformWidth/2+1, offsetY), stage);
                GeterateSide(new Vector2Int(_platformWidth/2+1, offsetY), stage, true);

                offsetY -= result.GetLength(1);
                equipment.Add(stageItems);
            }

            return equipment;
        }

        private (DungeonCellType[,], List<GameObject>) GeterateSide(Vector2Int offset, GeneratorStage stage, bool mirrored = false)
        {
            var result = dungeonProvider.GenerateCave();
            var stageList = new List<GameObject>();

            for (var x = 0; x < result.GetLength(0); x++)
            {
                for (var y = 0; y < result.GetLength(1); y++)
                {
                    var tilePosition = new Vector3Int(!mirrored?(x + offset.x):(-x - offset.x), y + offset.y, 0);
                    switch (result[x, y])
                    {
                        case DungeonCellType.Wall:
                            tilemap.SetTile(tilePosition, _wallTile);
                            break;
                    }
                }
            }


            foreach (var mineralSpawnPoint in dungeonProvider.GetMineralSpawnPoints(stage.MineralsCount))
            {
                Vector2 sp;
                if (mirrored)
                    sp = new Vector2(-mineralSpawnPoint.x - offset.x, mineralSpawnPoint.y + offset.y);
                else
                    sp = new Vector2(mineralSpawnPoint.x + offset.x, mineralSpawnPoint.y + offset.y);

                stageList.Add(Instantiate(mineral, sp, Quaternion.identity));
            }

            foreach (var weaponSpawnPoint in dungeonProvider.GetWeaponSpawnPoints(stage.WeaponsCount))
            {
                Vector2 sp;
                if (mirrored)
                    sp = new Vector2(-weaponSpawnPoint.x - offset.x, weaponSpawnPoint.y + offset.y);
                else
                    sp = new Vector2(weaponSpawnPoint.x + offset.x, weaponSpawnPoint.y + offset.y);

                var weaponObject = Instantiate(weapon, sp, Quaternion.identity);
                weaponObject.GetComponent<WorldWeapon>().GenerateGun(stage.WeaponRarity);
                stageList.Add(weaponObject);

            }

            return (result, stageList);
        }
    }
}