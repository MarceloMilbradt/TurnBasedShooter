using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private int turnNumber;
    public static TurnSystem Instance { get; private set; }
    public event EventHandler<int> OnTurnChange;
    private bool isPlayerTurn = true;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than one TurnSystem " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public int GetTurn()
    {
        return turnNumber;
    }
    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        OnTurnChange?.Invoke(this, turnNumber);
    }
    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
