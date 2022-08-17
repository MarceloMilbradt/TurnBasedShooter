using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private List<Unit> units;
    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        units = new List<Unit>();
    }
    public void AddUnit(Unit unit)
    {
        units.Add(unit);
    }
    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }
    public List<Unit> GetUnitList()
    {
        return units;
    }
    public bool HasAnyUnit()
    {
        return units.Count > 0;
    }
    public Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return units[0];
        }
        return null;
    }
    public GridPosition GetPosition() => gridPosition;
    public override string ToString()
    {
        var position = gridPosition.ToString();
        return $"{position}\n{units.Count}";
    }
}
