using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro fCostText;
    [SerializeField] private SpriteRenderer isWalkableSpriteRenderer;
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
        gCostText.text = node.GetGCost().ToString();
        fCostText.text = node.GetFCost().ToString();
        hCostText.text = node.GetHCost().ToString();
        isWalkableSpriteRenderer.color = node.IsWalkable() ? new Color(0,1,0,0.1f) : new Color(1, 0, 0, 0.1f);
    }
}
