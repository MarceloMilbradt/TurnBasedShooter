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
        EnemyAIAction bestAIAction = null;
        BaseAction bestAction = null;
        foreach (var baseAction in enemy.GetBaseActionArray())
        {
            if (!enemy.CanSpendActionPointsToTakeAction(baseAction))
            {
                continue;
            }
            if (bestAIAction == null)
            {
                bestAIAction = baseAction.GetBestEnemyAIAction();
                bestAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestAIAction.actionValue)
                {
                    bestAIAction = testEnemyAIAction;
                    bestAction = baseAction;
                }
            }
        }

        if (bestAIAction != null && enemy.TrySpendActionPointsToTakeAction(bestAction))
        {
            bestAction.TakeAction(bestAIAction.gridPosition, onEnemyActionCompleted);
            return true;
        }
        return false;

    }
}
