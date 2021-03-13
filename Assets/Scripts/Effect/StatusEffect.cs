using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : AbstractEffect
{
    [Header("Settings")]
    [SerializeField] private AbstractStatus status;

    public override bool ExecuteEffect(Character caster, Vector3 targetPos, GameObject targetObj)
    {
        Character targetChar = targetObj.GetComponent<Character>();
        if (!targetChar) return false;
        return targetChar.GetStatuses().AddStatus(status);
    }
}
