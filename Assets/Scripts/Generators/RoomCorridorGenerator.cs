using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RoomCorridorGenerator", menuName = "Dungeon/Generators/Room and Corridor")]
public class RoomCorridorGenerator : DungeonGenerator
{
    public int roomCount = 5;
    public int minRoomSize = 4;
    public int maxRoomSize = 8;

    public override void Generate(GridMap map)
    {
        // 1. Reset map to solid walls
        map.FillAll(Tile.TileType.Wall);

        List<RectInt> rooms = new List<RectInt>();

        for (int i = 0; i < roomCount; i++)
        {
            int w = Random.Range(minRoomSize, maxRoomSize);
            int h = Random.Range(minRoomSize, maxRoomSize);
            int x = Random.Range(1, map.Width - w - 1);
            int y = Random.Range(1, map.Height - h - 1);

            RectInt newRoom = new RectInt(x, y, w, h);

            // Simple collision check: don't overlap rooms for a cleaner look
            bool overlaps = false;
            foreach (var r in rooms)
            {
                if (newRoom.Overlaps(r)) { overlaps = true; break; }
            }

            if (!overlaps)
            {
                CarveRoom(map, newRoom);

                if (rooms.Count > 0)
                {
                    Vector2Int start = Vector2Int.RoundToInt(rooms[rooms.Count - 1].center);
                    Vector2Int end = Vector2Int.RoundToInt(newRoom.center);
                    CarveCorridor(map, start, end);
                }
                rooms.Add(newRoom);
            }
        }
    }

    private void CarveRoom(GridMap map, RectInt room)
    {
        for (int x = room.x; x < room.xMax; x++)
            for (int y = room.y; y < room.yMax; y++)
                map.SetTileType(new Vector2Int(x, y), Tile.TileType.Floor);
    }

    private void CarveCorridor(GridMap map, Vector2Int start, Vector2Int end)
    {
        // Horizontal then Vertical
        for (int x = Mathf.Min(start.x, end.x); x <= Mathf.Max(start.x, end.x); x++)
            map.SetTileType(new Vector2Int(x, start.y), Tile.TileType.Floor);

        for (int y = Mathf.Min(start.y, end.y); y <= Mathf.Max(start.y, end.y); y++)
            map.SetTileType(new Vector2Int(end.x, y), Tile.TileType.Floor);
    }
}