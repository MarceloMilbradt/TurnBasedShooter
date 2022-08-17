using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [SerializeField] private LayerMask hitMask;
    private static MouseWorld instance;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        transform.position = GetPosition();
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.hitMask);
        return raycastHit.point;
    }
    public static Vector3 GetPosition(LayerMask hitMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, hitMask);
        return raycastHit.point;
    }
}
