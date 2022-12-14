using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public class OnShootEventAgrs : EventArgs
    {
        public Unit targetUnit;
        public Unit originUnit;
    }
    public event EventHandler<OnShootEventAgrs> OnShoot;
    public static event EventHandler<OnShootEventAgrs> OnAnyShoot;
    public event EventHandler OnStop;
    [SerializeField] private int maxShootDistance = 7;
    [SerializeField] private LayerMask obstaclesLayerMask;
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }
    private State state;
    private float stateTimer;
    private Unit target;
    private bool canShoot;
    public override string GetActionName()
    {
        return "Shoot";
    }

    private void Update()
    {
        if (!isActive) return;
        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:
                Vector3 aimDirection = (target.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if(canShoot)
                {
                    Shoot();
                    canShoot = false;
                }
                break;
            case State.Cooloff:
                break;
        }
        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventAgrs
        {
            targetUnit = target,
            originUnit = unit
        });
        OnAnyShoot?.Invoke(this, new OnShootEventAgrs
        {
            targetUnit = target,
            originUnit = unit
        });
        target.Damage(40);
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                stateTimer = .2f;
                break;
            case State.Shooting:
                state = State.Cooloff;
                stateTimer = .5f;
                break;
            case State.Cooloff:
                OnStop?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override void TakeAction(GridPosition target, Action action)
    {
        state = State.Aiming;
        stateTimer = 1f;
        this.target = LevelGrid.Instance.GetUnitAtPosition(target);
        canShoot = true;
        ActionStart(action);

    }
    public override List<GridPosition> GetValidActionsGridPositionList()
    {
        var unitGridPostion = unit.GetGridPosition();
        return GetValidActionsGridPositionList(unitGridPostion);
    }
    public List<GridPosition> GetValidActionsGridPositionList(GridPosition position)
    {
        List<GridPosition> validPositions = new List<GridPosition>();
        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = position + offsetGridPosition;
                bool isValid = LevelGrid.Instance.IsValidGridPosition(testGridPosition);
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if (testDistance > maxShootDistance) continue;
                if (!isValid) continue; //fora da grid
                if (!LevelGrid.Instance.HasAnyUnitOnPosition(testGridPosition)) continue; // nao tem uma unidade

                Unit unitAtPosition = LevelGrid.Instance.GetUnitAtPosition(testGridPosition);
                if (unitAtPosition.IsEnemy() == unit.IsEnemy()) continue;


                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(position);
                Vector3 shootDirection = (unitAtPosition.GetWorldPosition() - unitWorldPosition).normalized;
                float maxDistance = Vector3.Distance(unitWorldPosition, unitAtPosition.GetWorldPosition());
                Vector3 origin = unitWorldPosition + Vector3.up * 1.7f;

                if (Physics.Raycast(origin, shootDirection, maxDistance, obstaclesLayerMask)) continue; //tem algo na frente (parede, pilar...)

                validPositions.Add(testGridPosition);
            }
        }
        return validPositions;
    }
    public Unit GetTargetUnit()
    {
        return target;
    }
    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        var unit  = LevelGrid.Instance.GetUnitAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - unit.GetHealth()) * 100f)
        };
    }

    public int GetValidTargetCount(GridPosition gridPosition)
    {
        var validGridPositions = GetValidActionsGridPositionList(gridPosition);
        return validGridPositions.Count;
    }
}
