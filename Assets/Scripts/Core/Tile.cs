using UnityEngine;

[System.Serializable]
public class Tile
{
    public enum TileType { Wall, Floor, Door, Trap }
    public TileType Type;
    public Vector2Int Position;
    public Entity Occupant; 
    public bool IsExplored; 
}