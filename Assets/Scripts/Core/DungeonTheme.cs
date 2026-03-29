using UnityEngine;

[CreateAssetMenu(fileName = "NewDungeonTheme", menuName = "Dungeon/Theme")]
public class DungeonTheme : ScriptableObject
{
    [Header("Generator")]
    public DungeonGenerator generator;

    [Header("Prefabs")]
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    
    [Header("Special (Optional)")]
    public GameObject doorPrefab;
}