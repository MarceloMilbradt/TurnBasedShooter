using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro fCostText;
    private PathNode node;
    public override void SetGridObject(object pathNode)
    {
        node = (PathNode)pathNode;
        base.SetGridObject(pathNode);
    }
    protected override void Update()
    {
        base.Update();
        gCostText.text = node.GetGCost().ToString();
        fCostText.text = node.GetFCost().ToString();
        hCostText.text = node.GetHCost().ToString();
    }
}
