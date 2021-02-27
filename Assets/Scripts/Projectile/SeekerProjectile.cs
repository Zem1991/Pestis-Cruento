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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seekRange);
    }

    protected override void Update()
    {
        base.Update();
        Seek();
    }

    private void Seek()
    {
        if (homingTarget) return;
        Collider[] colliders = Physics.OverlapSphere(transform.position, seekRange);
        foreach (Collider forCol in colliders)
        {
            GameObject forObj = forCol.gameObject;
            if (!CheckValidHomingTarget(forObj)) continue;
            homingTarget = forObj;
            break;
        }
    }
}
