using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform ragdollRootBone;

    private HealthSystem healthSystem;
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDie += HealthSystem_OnDie;
    }

    private void HealthSystem_OnDie(object sender, System.EventArgs e)
    {
        var ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        var unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(ragdollRootBone);
    }
}
