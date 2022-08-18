using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private BaseAction[] baseActionArray;
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] private bool isEnemy;
    [SerializeField] private int actionPoints = 4;
    [SerializeField] private int maxActionPoints = 6;
    [SerializeField] private int restoreActionPointsAmount = 4;
    private void Awake()
    {
        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        healthSystem.OnDie += HealthSystem_OnDie;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }
    public T GetAction<T>() where T : BaseAction
    {
        foreach (var action in baseActionArray)
        {
            if (action is T actionOfType)
            {
                return actionOfType;
            }
        }
        return null;
    }

    private void HealthSystem_OnDie(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChange(object sender, int e)
    {
        RestoreActionPoints();
    }

    public void Damage(int damage)
    {
        healthSystem.Damage(damage);
    }

    private void Update()
    {
        var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (gridPosition != newGridPosition)
        {
            var oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }
    public BaseAction[] GetBaseActionArray() => baseActionArray;
    public GridPosition GetGridPosition() => gridPosition;

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (actionPoints == 0) return false;

        return (actionPoints >= baseAction.GetActionPointsCost());
    }
    public int SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        return actionPoints;
    }
    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        return false;
    }
    public int GetActionPonits()
    {
        return actionPoints;
    }

    public void RestoreActionPoints()
    {
        if (ShouldRestorePoints())
        {
            actionPoints = Mathf.Clamp(actionPoints + restoreActionPointsAmount, 0, maxActionPoints);
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool ShouldRestorePoints()
    {
        return (IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn());
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    public float GetHealth()
    {
        return healthSystem.GetHealthNormalized();
    }
}
