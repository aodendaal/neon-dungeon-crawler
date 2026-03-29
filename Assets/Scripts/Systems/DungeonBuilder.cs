using UnityEngine;
using QFSW.QC;

public class DungeonBuilder : MonoBehaviour
{
    public DungeonTheme currentTheme;
    public GameObject playerPrefab;
    private GameObject dungeonContainer;
    private GameObject activePlayer;

    [Command("build-dungeon")]
    public void Build()
    {
        if (dungeonContainer != null) Destroy(dungeonContainer);
        dungeonContainer = new GameObject("DungeonContainer");
        dungeonContainer.transform.SetParent(this.transform);

        GridMap map = GridMap.Instance;
        currentTheme.generator.Generate(map);

        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                Tile tile = map.GetTile(x, y);
                Vector3 pos = new Vector3(x, 0, y);
                GameObject prefab = (tile.Type == Tile.TileType.Wall) ? currentTheme.wallPrefab : currentTheme.floorPrefab;

                if (prefab != null)
                    Instantiate(prefab, pos, Quaternion.identity, dungeonContainer.transform);
            }
        }

        SpawnPlayer(map);
        MinimapDisplay.Instance.Initialize(map.Width, map.Height);
    }

    private void SpawnPlayer(GridMap map)
    {
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                if (map.GetTile(x, y).Type == Tile.TileType.Floor)
                {
                    Vector3 spawnPos = new Vector3(x, 0, y);
                    activePlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
                    
                    Entity ent = activePlayer.GetComponent<Entity>();
                    ent.GridPosition = new Vector2Int(x, y);
                    map.GetTile(ent.GridPosition).Occupant = ent;

                    CameraManager.Instance.LinkPlayer(activePlayer.transform);
                    return;
                }
            }
        }
    }
}