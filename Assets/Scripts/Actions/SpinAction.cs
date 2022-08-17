using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    float totalSpinAmout;
    private void Update()
    {
        if (!isActive) return;

        float spinAmount = 360 * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);
        totalSpinAmout += spinAmount;
        if (totalSpinAmout >= 360)
        {
            ActionComplete();
        }

    }
    public override void TakeAction(GridPosition target, Action action)
    {
        totalSpinAmout = 0;
        ActionStart(action);
    }
    public override string GetActionName()
    {
        return "Spin";
    }
    public override List<GridPosition> GetValidActionsGridPositionList()
    {
        var unitGridPostion = unit.GetGridPosition();
        return new List<GridPosition> { unitGridPostion };
    }
    
}
