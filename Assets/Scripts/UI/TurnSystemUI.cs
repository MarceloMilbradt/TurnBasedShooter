using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject enemyTurnVisual;
    [SerializeField] private GameObject actionBarVisual;
    public void Start()
    {
        button.onClick.AddListener(() =>
        {
            UpdateTurn();
        });
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        UpdateEnemyTurnVisual();
        UpdateEndButtonVisibility();
    }

    private void TurnSystem_OnTurnChange(object sender, int e)
    {
        UpdateTurnNumber(e);
        UpdateEnemyTurnVisual();
        UpdateEndButtonVisibility();
    }

    private void UpdateTurn()
    {
        TurnSystem.Instance.NextTurn();
    }
    private void UpdateTurnNumber(int turn)
    {
        textMeshPro.text = "TURN " + turn;
    }
    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisual.SetActive(!TurnSystem.Instance.IsPlayerTurn());
        actionBarVisual.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
    private void UpdateEndButtonVisibility()
    {
        button.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
