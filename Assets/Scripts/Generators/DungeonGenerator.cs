using UnityEngine;

public abstract class DungeonGenerator : ScriptableObject
{
    // The "Contract" every generator must fulfill
    public abstract void Generate(GridMap map);
}