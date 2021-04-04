using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisEffect : AbstractEffect
{
    [Header("Settings")]
    [SerializeField] private float strength;

    public override bool ExecuteEffect(Character caster, Vector3 targetPos, GameObject targetObj)
    {
        ITelekinesisTarget tkTarget = targetObj.GetComponent<ITelekinesisTarget>();
        if (tkTarget != null)
        {
            tkTarget.ApplyTelekinesis();
            return true;
        }

        Character targetChar = targetObj.GetComponent<Character>();
        if (targetChar)
        {
            //Vector3 direction = (targetChar.transform.position - targetPos).normalized;
            //Vector3 impact = direction * strength;
            //targetChar.ReceiveImpact(impact);
            targetChar.ReceiveImpact(strength, targetPos, 0);
            return true;
        }
        
        return false;
    }
}
