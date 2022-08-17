using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStart;
    public event EventHandler OnStop;
    [SerializeField] private int maxDistance = 4;

    private Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        float stoppingDistance = .1f;
        bool isMoving = Vector3.Distance(transform.position, targetPosition) >= stoppingDistance;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if (isMoving)
        {
            float moveSpeed = 4f;
            transform.position += moveSpeed * Time.deltaTime * moveDirection;
        }
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        if (!isMoving)
        {
            ActionComplete();
            OnStop?.Invoke(this, EventArgs.Empty);
        }
    }
   
    public override List<GridPosition> GetValidActionsGridPositionList()
    {
        List<GridPosition> validPositions = new List<GridPosition>();
        var unitGridPostion = unit.GetGridPosition();
        for (int x = -maxDistance; x <= maxDistance; x++)
        {
            for (int z = -maxDistance; z <= maxDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPostion + offsetGridPosition;
                bool isValid = LevelGrid.Instance.IsValidGridPosition(testGridPosition);
                if (!isValid) continue; //fora da grid
                if (unitGridPostion == testGridPosition) continue; //na posiçao que já está
                if (LevelGrid.Instance.HasAnyUnitOnPosition(testGridPosition)) continue; // ja tem uma unidade
                validPositions.Add(testGridPosition);
            }
        }
        return validPositions;
    }
    public override void TakeAction(GridPosition target, Action action)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(target);
        ActionStart(action);
        OnStart?.Invoke(this, EventArgs.Empty);
    }

    public override string GetActionName()
    {
        return "Move";
    }
}
