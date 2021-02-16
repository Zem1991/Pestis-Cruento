using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisEffect : AbstractEffect
{
    [Header("Settings")]
    [SerializeField] private float strength;

    public override bool ExecuteEffect(Character caster, Vector3 targetPos, Character targetChar)
    {
        if (targetChar)
        {
            Vector3 direction = (targetChar.transform.position - targetPos).normalized;
            Vector3 impact = direction * strength;
            targetChar.ReceiveImpact(impact);
        }
        return true;
    }
}
