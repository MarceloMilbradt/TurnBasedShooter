using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;
    public void Setup(Transform originarRootBone)
    {
        MatchAllChildTransforms(originarRootBone, ragdollRootBone);
        ApplyExplosion(ragdollRootBone, 300, transform.position, 10f);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            var cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }
    private void ApplyExplosion(Transform originalRoot, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in originalRoot)
        {
            if (child.TryGetComponent(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
                ApplyExplosion(child, explosionForce, explosionPosition, explosionRange);
            }
        }
    }
}
