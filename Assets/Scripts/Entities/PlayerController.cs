using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Entity _playerEntity;

    private void Awake()
    {
        _playerEntity = GetComponent<Entity>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Only trigger on the 'Performed' phase (initial press)
        if (!context.performed) return;

        Vector2 input = context.ReadValue<Vector2>();

        // Rotation: A and D keys
        if (Mathf.Abs(input.x) > 0.9f)
        {
            int rotationAngle = input.x > 0 ? 90 : -90;
            _playerEntity.TryRotate(rotationAngle);
            RevealFogOfWar();
            TickManager.Instance.PlayerActionPulse();
        }
        // Movement: W and S keys
        else if (Mathf.Abs(input.y) > 0.9f)
        {
            Vector2Int moveDir = _playerEntity.Facing;
            if (input.y < 0) moveDir *= -1; // Invert for backwards

            if (_playerEntity.TryMove(moveDir))
            {
                RevealFogOfWar();
                TickManager.Instance.PlayerActionPulse();
            }
        }
    }

    private void RevealFogOfWar()
    {
        if (GridMap.Instance == null) return;
        
        Vector2Int pos = _playerEntity.GridPosition;
        Vector2Int forward = _playerEntity.Facing;

        GridMap.Instance.GetTile(pos).IsExplored = true;

        for (int i = 1; i <= 3; i++)
        {
            Vector2Int checkPos = pos + (forward * i);
            var tile = GridMap.Instance.GetTile(checkPos);
            if (tile != null)
            {
                tile.IsExplored = true;
                if (tile.Type == Tile.TileType.Wall) break;
            }
        }
    }
}