using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerProjectile : Projectile
{
    [Header("Seeker")]
    [SerializeField] private float seekRange = 5F;

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = GizmoColors.projectileSeekRange;
        Gizmos.DrawWireSphere(transform.position, seekRange);
    }

    protected override void Update()
    {
        base.Update();
        Seek();
    }

    private void Seek()
    {
        if (homingSpeed <= 0) return;
        if (homingTarget != null) return;
        Collider[] colliders = Physics.OverlapSphere(transform.position, seekRange);
        foreach (Collider forCol in colliders)
        {
            GameObject forObj = forCol.gameObject;
            if (!CheckValidHomingTarget(forObj)) continue;

            ITargetable targetable = forObj.GetComponent<ITargetable>();
            homingTarget = targetable;
            break;
        }
    }
}
