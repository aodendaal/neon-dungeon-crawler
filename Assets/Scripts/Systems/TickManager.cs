using UnityEngine;
using QFSW.QC;

public class TickManager : MonoBehaviour
{
    public static TickManager Instance { get; private set; }
    public bool IsGameRunning { get; private set; } = false;
    [SerializeField] private float tickRate = 1.0f;
    private float _timer;

    private void Awake() => Instance = this;

    private void Update()
    {
        if (!IsGameRunning) return;
        _timer += Time.deltaTime;
        if (_timer >= tickRate) { _timer = 0; Pulse(); }
    }

    public void Pulse()
    {
        // NEW: The Unity 6 way to find all active scripts that might be ITickReceivers
        // We exclude inactive objects to prevent "ghost" ticks from destroyed monsters
        var receivers = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude);

        foreach (var receiver in receivers)
        {
            if (receiver is ITickReceiver tickable)
            {
                tickable.Tick();
            }
        }

        // Refresh the UI so the player icon and explored tiles update
        if (MinimapDisplay.Instance != null && MinimapDisplay.Instance.IsInitialized)
        {
            MinimapDisplay.Instance.RefreshMap();
        }
    }

    [Command("start-game")] public void StartGame() => IsGameRunning = true;

    public void PlayerActionPulse()
    {
        // 1. Reset the auto-tick timer so the world doesn't 
        // "double tick" immediately after a player move.
        _timer = 0;

        // 2. Trigger the world update
        Pulse();
    }
}