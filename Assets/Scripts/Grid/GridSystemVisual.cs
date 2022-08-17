using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridVisualTransform;
    private GridVisualSingle[,] gridVisuals;
    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType visualType;
        public Material material;
    }
    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        Green,
        Yellow
    }
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterials;
    void Start()
    {
        int width = LevelGrid.Instance.GetWidth();
        int hight = LevelGrid.Instance.GetHeight();
        gridVisuals = new GridVisualSingle[width, hight];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < hight; z++)
            {
                GridPosition pos = new GridPosition(x, z);
                var transform = Instantiate(gridVisualTransform, LevelGrid.Instance.GetWorldPosition(pos), Quaternion.identity);
                gridVisuals[x, z] = transform.GetComponent<GridVisualSingle>();
            }
        }
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, System.EventArgs e)
    {
        UpdateGridVisual();
    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, System.EventArgs e)
    {
        UpdateGridVisual();
    }

    private void UnitActionSystem_OnSelectedActionChange(object sender, System.EventArgs e)
    {
        UpdateGridVisual();
    }

    public void HideAllGridPositions()
    {
        int width = LevelGrid.Instance.GetWidth();
        int hight = LevelGrid.Instance.GetHeight();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < hight; z++)
            {
                gridVisuals[x, z].Hide();
            }
        }
    }
    public void ShowGridPositions(List<GridPosition> positions, GridVisualType visualType)
    {
        foreach (GridPosition pos in positions)
        {

            gridVisuals[pos.x, pos.z].Show(GetGridVisualMaterial(visualType));
        }

    }

    public void UpdateGridVisual()
    {
        HideAllGridPositions();
        var baseAction = UnitActionSystem.Instance.GetSelectedAction();
        var unit = UnitActionSystem.Instance.GetSelectedUnit();
        if (baseAction == null)
            return;

        var selectedVisual = GridVisualType.White;
        switch (baseAction)
        {
            case MoveAction action:
                selectedVisual = GridVisualType.White;
                break;
            case ShootAction action:
                selectedVisual = GridVisualType.Red;
                ShowGridPositionRange(unit.GetGridPosition(), action.GetMaxShootDistance(), GridVisualType.Yellow);
                break;      
            case SpinAction action:
                selectedVisual = GridVisualType.Blue;
                break;
            default:
                selectedVisual = GridVisualType.White;
                break;
        };
        ShowGridPositions(baseAction.GetValidActionsGridPositionList(), selectedVisual);
    }
    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType visualType)
    {
        List<GridPosition> gridPositions = new List<GridPosition>();
        for (int x = -range; x < range; x++)
        {
            for (int z = -range; z < range; z++)
            {
                GridPosition position = gridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(position)) continue;
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if (testDistance > range) continue;
                gridPositions.Add(position);
            }
        }

        ShowGridPositions(gridPositions, visualType);
    }
    private Material GetGridVisualMaterial(GridVisualType gridVisual)
    {
        foreach (GridVisualTypeMaterial visualTypeMaterial in gridVisualTypeMaterials)
        {
            if (visualTypeMaterial.visualType == gridVisual)
            {
                return visualTypeMaterial.material;
            }
        }
        throw new Exception("Material Not Found");
    }
}
