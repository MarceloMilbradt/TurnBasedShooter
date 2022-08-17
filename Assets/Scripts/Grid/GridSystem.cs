using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int height;
    private int width;
    private float cellSize;
    private GridObject[,] gridObjects;
    public GridSystem(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridObjects = new GridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var gridPosition = new GridPosition(x, z);
                gridObjects[x, z] = new GridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition position)
    {
        return new Vector3(position.x, 0, position.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPos)
    {
        return new GridPosition(worldPos.x / cellSize, worldPos.z / cellSize);
    }
    public GridObject GetGridObject(GridPosition position)
    {
        return gridObjects[position.x, position.z];
    }
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var position = new GridPosition(x, z);
                Transform debugTransform = Object.Instantiate(debugPrefab, GetWorldPosition(position), Quaternion.identity);
                GridDebugObject gridDebug = debugTransform.GetComponent<GridDebugObject>();
                gridDebug.SetGridObject(GetGridObject(position));
            }
        }
    }
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
               gridPosition.z >= 0 &&
               gridPosition.x < width &&
               gridPosition.z < height;
    }
    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
}
