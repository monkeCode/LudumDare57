using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonProvider : MonoBehaviour
{
    [SerializeField] private DungeonGenerator generator;
    [SerializeField] private float objectSpawnProtectRadius = 10f;

    public DungeonCellType[,] GenerateCave()
    {
        return generator.GenerateCave();
    }

    public Vector3[] GetWeaponSpawnPoints(int weaponsCount)
    {
        var cave = generator.CaveCells;
        var caveWidth = cave.GetLength(0);
        var caveHeight = cave.GetLength(1);
        
        var points = new List<Vector2Int>();

        while (points.Count < weaponsCount * 2)
        {
            var x = Random.Range(0, caveWidth);
            var y = Random.Range(0, caveHeight);
            
            
            if (cave[x, y] != DungeonCellType.Empty)
                continue;

            while (cave[x, y - 1] != DungeonCellType.Wall)
            {
                y--;
            }
            
            var point = new Vector2Int(x, y);
            if (!HasCloseAnotherObject(point, points))
                points.Add(point);
        }

        var furthest = points
            .OrderByDescending(p => Vector2.Distance(p, new Vector2Int(generator.startPosX, generator.startPosY)))
            .Take(weaponsCount)
            .ToArray();

        var parentPosition = transform.position;
        return furthest.Select(p => parentPosition + new Vector3(p.x + 1, p.y + 1, -1)).ToArray();
    }

    public Vector3[] GetMineralSpawnPoints(int mineralCount)
    {
        var cave = generator.CaveCells;
        var caveWidth = cave.GetLength(0);
        var caveHeight = cave.GetLength(1);
        
        var points = new List<Vector2Int>();
        
        while (points.Count < mineralCount)
        {
            var x = Random.Range(0, caveWidth);
            var y = Random.Range(0, caveHeight);
            
            if (cave[x, y] != DungeonCellType.Empty)
                continue;
            
            var point = new Vector2Int(x, y);
            if (!HasCloseAnotherObject(point, points))
                points.Add(point);
        }

        var parentPosition = transform.position;
        return points.Select(p => parentPosition + new Vector3(p.x + 1, p.y + 1, -1)).ToArray();
    }

    private bool HasCloseAnotherObject(Vector2Int pos, List<Vector2Int> points)
    {
        if (points.Count == 0)
            return false;
        var closest = points.Select(p => Vector2.Distance(p, pos)).Min();
        return closest <= objectSpawnProtectRadius;
    }
}
