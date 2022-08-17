using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected bool isActive;
    protected Unit unit;
    protected Action actionCompleted;
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionComplete;
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }
    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition position, Action onActionCompleted);
    public virtual bool IsValidActionGridPosition(GridPosition position)
    {
        var validPositions = GetValidActionsGridPositionList();
        return validPositions.Contains(position);
    }
    public abstract List<GridPosition> GetValidActionsGridPositionList();
    public virtual int GetActionPointsCost()
    {
        return 2;
    }
    protected void ActionStart(Action action)
    {
        isActive = true;
        actionCompleted = action;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }
    protected void ActionComplete()
    {
        isActive = false;
        actionCompleted();
        OnAnyActionComplete?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetUnit()
    {
        return unit;
    }
}
