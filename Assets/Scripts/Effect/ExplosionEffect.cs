using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : AbstractEffect
{
    [Header("Settings")]
    [SerializeField] private int radius;
    [SerializeField] private int damage;
    [SerializeField] private float strength;

    public override bool ExecuteEffect(Character caster, Vector3 targetPos, GameObject targetObj)
    {
        Collider[] colliders = Physics.OverlapSphere(targetPos, radius);
        foreach (Collider forColl in colliders)
        {
            Character forChar = forColl.GetComponent<Character>();
            if (!forChar) continue;

            Vector3 forCharPos = forChar.transform.position + forChar.GetTargetablePosition();
            float distance = Vector3.Distance(forCharPos, targetPos);
            float distanceFactor = (radius - distance) / radius;
            if (distanceFactor < 0) distanceFactor = 0;

            float damageFloat = damage * distanceFactor;
            int damageInt = Mathf.RoundToInt(damageFloat);

            Vector3 direction = (forCharPos - targetPos).normalized;
            float strengthAdjusted = strength * distanceFactor;
            Vector3 impact = direction * strengthAdjusted;

            //Debug.Log("distanceFactor: " + distanceFactor);
            //Debug.Log("impact.magnitude: " + impact.magnitude);

            forChar.LoseHealth(damageInt);
            forChar.ReceiveImpact(impact);
        }
        return true;
    }
}
