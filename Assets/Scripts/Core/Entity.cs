using UnityEngine;
using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using QFSW.QC;

public class Entity : MonoBehaviour, ITickReceiver
{
    public Vector2Int GridPosition;
    public Vector2Int Facing = Vector2Int.up;
    public int currentHP = 10;

    [Header("Feel Feedbacks")]
    [SerializeField] protected MMF_Player _moveFeedback;
    [SerializeField] protected MMF_Player _rotateFeedback;
    [SerializeField] protected MMF_Player _damageFeedback;

    public event Action OnMoved;
    public event Action OnRotated;

    public virtual void Tick() { /* Logic for status effects later */ }

    public bool TryMove(Vector2Int direction)
    {
        Vector2Int targetPos = GridPosition + direction;
        if (GridMap.Instance.IsWalkable(targetPos))
        {
            GridMap.Instance.GetTile(GridPosition).Occupant = null;
            GridPosition = targetPos;
            GridMap.Instance.GetTile(GridPosition).Occupant = this;

            if (_moveFeedback != null)
            {
                var posAnim = _moveFeedback.GetFeedbackOfType<MMF_Position>();
                if (posAnim != null)
                {
                    posAnim.DestinationPosition = new Vector3(GridPosition.x, 0, GridPosition.y);
                    _moveFeedback.PlayFeedbacks();
                }
            }
            OnMoved?.Invoke();
            return true;
        }
        return false;
    }

    public void TryRotate(int angle)
    {
        float rad = -angle * Mathf.Deg2Rad;
        int newX = Mathf.RoundToInt(Facing.x * Mathf.Cos(rad) - Facing.y * Mathf.Sin(rad));
        int newY = Mathf.RoundToInt(Facing.x * Mathf.Sin(rad) + Facing.y * Mathf.Cos(rad));
        Facing = new Vector2Int(newX, newY);

        if (_rotateFeedback != null) _rotateFeedback.PlayFeedbacks();
        // Fallback if no feedback is set
        else transform.Rotate(0, angle, 0);

        OnRotated?.Invoke();
    }
}