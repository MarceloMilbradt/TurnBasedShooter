using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridVisualTransform;
    private GridVisualSingle[,] gridVisuals;

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
    }

    private void Update()
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
    public void ShowGridPositions(List<GridPosition> positions)
    {
        foreach (GridPosition pos in positions)
        {

            gridVisuals[pos.x, pos.z].Show();
        }

    }

    public void UpdateGridVisual()
    {
        HideAllGridPositions();
        var baseAction = UnitActionSystem.Instance.GetSelectedAction();
        if (baseAction == null)
            return;
        ShowGridPositions(baseAction.GetValidActionsGridPositionList());
    }

}
