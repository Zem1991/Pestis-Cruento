using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpell : AbstractSpell
{
    [Header("Effect")]
    [SerializeField] private AbstractEffect effect;

    //public override bool CanUse(MainCharacter caster, Vector3 targetPos, Character targetChar)
    //{
    //    if (!base.CanUse(user)) return false;
    //    if (user.CheckFullHealth()) return false;
    //    return true;
    //}

    public override bool Cast(MainCharacter caster, Vector3 targetPos, GameObject targetObj)
    {
        if (!base.Cast(caster, targetPos, targetObj)) return false;
        Vector3 position = caster.transform.position;
        effect.ExecuteEffect(caster, position, targetObj);
        return true;
    }
}
