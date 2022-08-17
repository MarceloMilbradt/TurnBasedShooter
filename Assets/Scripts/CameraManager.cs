using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCamera;
    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionComplete += BaseAction_OnAnyActionComplete;
    }

    private void BaseAction_OnAnyActionComplete(object sender, System.EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionStarted(object sender, System.EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                LookAtTarget(shootAction);
                ShowActionCamera();
                break;
        }
    }

    private void LookAtTarget(ShootAction shootAction)
    {
        var originUnit = shootAction.GetUnit();
        var targetUnit = shootAction.GetTargetUnit();
        Vector3 cameraHeight = Vector3.up * 1.7f;
        Vector3 shootDir = (targetUnit.GetWorldPosition() - originUnit.GetWorldPosition()).normalized;

        float shoulderOffsetAmmout = 0.5f;
        Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmmout;
        Vector3 actionCameraPosition = originUnit.GetWorldPosition() + cameraHeight + shoulderOffset + (shootDir * -1);
        actionCamera.transform.position = actionCameraPosition;
        actionCamera.transform.LookAt(targetUnit.GetWorldPosition() + cameraHeight);
    }

    private void ShowActionCamera()
    {
        actionCamera.SetActive(true);
    }
    private void HideActionCamera()
    {
        actionCamera.SetActive(false);
    }
}
