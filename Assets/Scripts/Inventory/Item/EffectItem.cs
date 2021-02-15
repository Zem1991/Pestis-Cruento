using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectItem : AbstractItem
{
    [Header("Effect")]
    [SerializeField] private AbstractEffect effect;

    //public override bool CanUse(Character user, Vector3 targetPos, Character targetChar)
    //{
    //    if (!base.CanUse(user)) return false;
    //    if (user.CheckFullHealth()) return false;
    //    return true;
    //}

    public override bool Use(Character user, Vector3 targetPos, Character targetChar)
    {
        if (!base.Use(user, targetPos, targetChar)) return false;
        Vector3 position = user.transform.position;
        effect.ExecuteEffect(user, position, user);
        return true;
    }
}
