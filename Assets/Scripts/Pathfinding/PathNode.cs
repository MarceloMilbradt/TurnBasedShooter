using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode cameFromNode;
    private bool isWalkable = true;
    public PathNode(GridPosition position)
    {
        gridPosition = position;
    }
    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public int GetGCost() { return gCost; }
    public int GetHCost() { return hCost; }
    public int GetFCost() { return fCost; }

    public void SetGCost(int value) { gCost = value; }
    public void SetHCost(int value) { hCost = value; }
    public void CalculateFCost() { fCost = gCost + hCost; }

    public void ResetCameFromNode()
    {
        cameFromNode = null;
    }
    public void SetCameFromNode(PathNode node)
    {
        cameFromNode = node;
    }
    public PathNode GetCameFrom()
    {
        return cameFromNode;
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public bool IsWalkable()
    {
        return isWalkable;
    }
    public void SetWalkable(bool value)
    {
        isWalkable = value;
    }
}
