using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpell : AbstractSpell
{
    [Header("Projectile")]
    [SerializeField] private Projectile projectile;

    //public override bool CanUse(MainCharacter caster, Vector3 targetPos, Character targetChar)
    //{
    //    if (!base.CanUse(user)) return false;
    //    if (user.CheckFullHealth()) return false;
    //    return true;
    //}

    public override bool Cast(MainCharacter caster, Vector3 targetPos, Character targetChar)
    {
        if (!base.Cast(caster, targetPos, targetChar)) return false;
        caster.SpawnProjectile(projectile, targetChar);
        return true;
    }
}
