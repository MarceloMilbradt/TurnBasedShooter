using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    [SerializeField] private GameObject actionBar;
    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool busy)
    {
        UpdateActionBar(busy);
    }

    public void UpdateActionBar(bool busy)
    {
        actionBar.SetActive(!busy);
    }
}
