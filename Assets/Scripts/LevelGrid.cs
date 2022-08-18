using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }
    private GridSystem<GridObject> gridSystem;
    [SerializeField] Transform debugTransform;
    public event EventHandler OnAnyUnitMovedGridPosition;
    [SerializeField] private bool debug;

    [SerializeField]  private int width;
    [SerializeField]  private int height;
    [SerializeField]  private float cellSize;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than one LevelGrid " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (grid, position) => new GridObject(grid, position));
        if (debug)
            gridSystem.CreateDebugObjects(debugTransform);
    }
    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
    }
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public void GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }
    public void UnitMovedGridPosition(Unit unit, GridPosition from, GridPosition to)
    {
        RemoveUnitAtGridPosition(from, unit);
        AddUnitAtGridPosition(to, unit);
        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }
    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition position) => gridSystem.GetWorldPosition(position);
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    public bool HasAnyUnitOnPosition(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }
    public Unit GetUnitAtPosition(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
}
