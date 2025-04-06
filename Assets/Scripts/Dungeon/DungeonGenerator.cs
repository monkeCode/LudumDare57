using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Size options")]
    [SerializeField] private int caveWidth = 200;
    [SerializeField] private int caveHeight = 100;
    [SerializeField] private int minRoomWidth = 10;
    [SerializeField] private int maxRoomWidth = 35;
    [SerializeField] private int minRoomHeight = 2;
    [SerializeField] private int maxRoomHeight = 30;
    private const int RoomPlacementAttempts = 3000;
    
    [Header("Player starting position")]
    public int startPosX = 0;
    public int startPosY = 30;
    
    [Header("Tunnels options")]
    [SerializeField] private int connectionCount = 15;
    [SerializeField] private float tunnelRadius = 3;
    [SerializeField] private float alternateTunnelChance = 0.2f;
    [SerializeField] private int alternateTunnelMaxLength = 3;
    [SerializeField] private float maxAngleToConnect = 30f;
    
    private readonly HashSet<Vector2Int> visitedPoints = new();
    private readonly List<(Vector2Int, Vector2Int)> lines = new();
    
    public DungeonCellType[,] CaveCells { get; private set; }
    
    public DungeonCellType[,] GenerateCave()
    {
        visitedPoints.Clear();
        lines.Clear();
        
        CaveCells = new DungeonCellType[caveWidth, caveHeight];
        for (var x = 0; x < caveWidth; x++)
            for (var y = 0; y < caveHeight; y++)
                CaveCells[x, y] = DungeonCellType.Wall;
        
        var rooms = new List<RectInt>();
        var rnd = new System.Random();

        for (var i = 0; i < RoomPlacementAttempts; i++)
        {
            var roomWidth = rnd.Next(minRoomWidth, maxRoomWidth + 1);
            var roomHeight = rnd.Next(minRoomHeight, maxRoomHeight + 1);
            var x = rnd.Next(1, caveWidth - roomWidth - 1);
            var y = rnd.Next(1, caveHeight - roomHeight - 1);
            var newRoom = new RectInt(x, y, roomWidth, roomHeight);
            
            if (rooms.Count == 0)
            {
                rooms.Add(newRoom);
                continue;
            }

            if (rooms.Any(newRoom.Overlaps))
                continue;

            while (!rooms.Any(newRoom.Overlaps)) 
                newRoom = new RectInt(newRoom.x - 1, newRoom.y - 1, newRoom.width + 2, newRoom.height + 2);
            
            if (newRoom.x + newRoom.width > caveWidth - 1 || newRoom.y + newRoom.height > caveHeight - 1 || newRoom.x < 0 || newRoom.y < 0)
                continue;
            if (newRoom.width > maxRoomWidth || newRoom.height > maxRoomHeight)
                continue;
            
            rooms.Add(newRoom);
        }
        
        var points = rooms.Select(roomRect => new Vector2Int(roomRect.x + roomRect.width / 2, roomRect.y + roomRect.height / 2)).ToList();
        var currentPoint = new Vector2Int(startPosX, startPosY);
        ConnectPoints(points, currentPoint, connectionCount);


        for (var i = 0; i < caveWidth; i++)
        for (var j = 0; j < caveHeight; j++)
        {
            foreach (var (a, b) in lines)
            {
                var dist = DistancePointToSegment(new Vector2Int(i, j), a, b);
                if (dist <= tunnelRadius)
                {
                    CaveCells[i, j] = DungeonCellType.Empty;
                    break;
                }
            }
        }
        
        for (var x = 0; x < caveWidth; x++)
        {
            CaveCells[x, 0] = DungeonCellType.Wall;
            CaveCells[x, caveHeight - 1] = DungeonCellType.Wall;
        }

        for (var y = 0; y < caveHeight; y++)
        {
            if (Vector2.Distance(new Vector2(0, y), new Vector2(startPosX, startPosY)) > tunnelRadius)
                CaveCells[0, y] = DungeonCellType.Wall;
            if (Vector2.Distance(new Vector2(caveWidth - 1, y), new Vector2(startPosX, startPosY)) > tunnelRadius)
                CaveCells[caveWidth - 1, y] = DungeonCellType.Wall;
                
        }
        return CaveCells;
    }

    private void ConnectPoints(List<Vector2Int> points, Vector2Int currentPoint, int connections)
    {
        visitedPoints.Add(currentPoint);
        var nextPoint = points
            .Where(p => !visitedPoints.Contains(p) && IsLowAngle(currentPoint, p))
            .OrderBy(p => Vector2Int.Distance(p, currentPoint))
            .FirstOrDefault();
        
        if (nextPoint == Vector2Int.zero)
            return;

        lines.Add((currentPoint, nextPoint));
        if (connections > 0)
            ConnectPoints(points, nextPoint, connections - 1);
        
        if (Random.value < alternateTunnelChance)
            ConnectPoints(points, currentPoint, Mathf.Min(alternateTunnelMaxLength, connections - 1));
        
    }

    private  bool IsLowAngle(Vector2Int p1, Vector2Int p2)
    {
        var angle = Vector2.Angle(p2 - p1, Vector2Int.right);
        return angle <= maxAngleToConnect || angle >= 180f - maxAngleToConnect;
    }
    
    private static float DistancePointToSegment(Vector2Int p, Vector2Int a, Vector2Int b)
    {
        Vector2 ap = p - a;
        Vector2 ab = b - a;

        var abSqr = ab.sqrMagnitude;
        var dot = Vector2.Dot(ap, ab);
        var t = Mathf.Clamp01(dot / abSqr);

        var closest = a + ab * t;
        return Vector2.Distance(p, closest);
    }
}