using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStart;
    public event EventHandler OnStop;
    [SerializeField] private int maxDistance = 4;

    private List<Vector3> positionLists;
    private int currentPositionIndex;
    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        float stoppingDistance = .1f;
        Vector3 targetPosition = positionLists[currentPositionIndex];
        bool isMoving = Vector3.Distance(transform.position, targetPosition) >= stoppingDistance;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        if (isMoving)
        {
            float moveSpeed = 4f;
            transform.position += moveSpeed * Time.deltaTime * moveDirection;
        }
        if (!isMoving)
        {
            currentPositionIndex++;
            if (currentPositionIndex >= positionLists.Count)
            {
                OnStop?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
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
                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition)) continue; //Nao é um lugar que pode ter unidades (paredes, pilares...)
                if (!Pathfinding.Instance.HasPath(unitGridPostion, testGridPosition)) continue; //Não é alcançavel

                var pathLength = Pathfinding.Instance.GetPathLength(unitGridPostion, testGridPosition);

                if (pathLength > maxDistance * 10) continue; //Muito Longe

                validPositions.Add(testGridPosition);
            }
        }
        return validPositions;
    }
    public override void TakeAction(GridPosition target, Action action)
    {
        List<GridPosition> gridPositions = Pathfinding.Instance.FindPath(unit.GetGridPosition(), target, out _);
        currentPositionIndex = 0;
        positionLists = new List<Vector3>();
        foreach (var position in gridPositions)
        {
            positionLists.Add(LevelGrid.Instance.GetWorldPosition(position));
        }
        OnStart?.Invoke(this, EventArgs.Empty);
        ActionStart(action);
    }

    public override string GetActionName()
    {
        return "Move";
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        var targetCount = unit.GetAction<ShootAction>().GetValidTargetCount(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCount * 10
        };
    }
}
