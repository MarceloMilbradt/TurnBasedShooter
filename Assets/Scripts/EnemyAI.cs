using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }
    private State state;
    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private float timer;
    void Start()
    {
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
    }

    private void TurnSystem_OnTurnChange(object sender, int e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        state = State.TakingTurn;
        timer = 2f;
    }

    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }

    }
    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }
    private bool TryTakeEnemyAIAction(Action onEnemyActionCompleted)
    {
        foreach (var enemy in UnitManager.Instance.GetEnemyUnits())
        {
            if (TryTakeEnemyAIAction(enemy, onEnemyActionCompleted))
            {
                return true;
            }
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemy, Action onEnemyActionCompleted)
    {
        var action = enemy.GetSpinAction();

        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return false;
        }

        GridPosition actionGridPosition = enemy.GetGridPosition();

        if (!action.IsValidActionGridPosition(actionGridPosition))
        {
            return false;
        }

        if (!enemy.TrySpendActionPointsToTakeAction(action))
        {
            return false;
        }

        action.TakeAction(actionGridPosition, onEnemyActionCompleted);
        return true;
    }
}
