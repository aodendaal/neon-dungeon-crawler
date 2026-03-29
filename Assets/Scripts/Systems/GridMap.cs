using UnityEngine;

public class GridMap : MonoBehaviour
{
    public static GridMap Instance { get; private set; }
    public int Width = 20;
    public int Height = 20;
    private Tile[,] map;

    private void Awake()
    {
        Instance = this;
        InitializeMap();
    }

    public void InitializeMap()
    {
        map = new Tile[Width, Height];
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                map[x, y] = new Tile { Position = new Vector2Int(x, y), Type = Tile.TileType.Wall };
    }

    public bool IsWalkable(Vector2Int pos) =>
        pos.x >= 0 && pos.x < Width && pos.y >= 0 && pos.y < Height &&
        map[pos.x, pos.y].Type != Tile.TileType.Wall && map[pos.x, pos.y].Occupant == null;

    public Tile GetTile(Vector2Int pos) => map[pos.x, pos.y];
    public Tile GetTile(int x, int y) => map[x, y];
    public void FillAll(Tile.TileType type)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                map[x, y].Type = type;
            }
        }
    }

    public void SetTileType(Vector2Int pos, Tile.TileType type)
    {
        if (pos.x >= 0 && pos.x < Width && pos.y >= 0 && pos.y < Height)
        {
            map[pos.x, pos.y].Type = type;
        }
    }
}