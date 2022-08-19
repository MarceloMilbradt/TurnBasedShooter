using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform bulletPorjectile;
    [SerializeField] Transform shootPoint;
    private void Awake()
    {
        if (TryGetComponent(out MoveAction moveAction))
        {
            moveAction.OnStart += MoveAction_onStart;
            moveAction.OnStop += MoveAction_onStop;
        }
        if (TryGetComponent(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnStart;
        }
    }

    private void ShootAction_OnStart(object sender, ShootAction.OnShootEventAgrs e)
    {
        animator.SetTrigger(Animations.SHOOT);
        var bulletTransform = Instantiate(bulletPorjectile, shootPoint.position, Quaternion.identity);
        var bullet = bulletTransform.GetComponent<BulletProjectile>();
        var targetPosition = e.targetUnit.GetWorldPosition();
        targetPosition.y = shootPoint.position.y;
        bullet.Setup(targetPosition);
    }

    private void MoveAction_onStop(object sender, System.EventArgs e)
    {
        animator.SetBool(Animations.WALKING, false);
    }

    private void MoveAction_onStart(object sender, System.EventArgs e)
    {
        animator.SetBool(Animations.WALKING, value: true);
    }
}
