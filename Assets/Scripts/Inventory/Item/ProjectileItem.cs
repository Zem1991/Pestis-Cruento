using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileItem : AbstractItem
{
    [Header("Projectile")]
    [SerializeField] private Projectile projectile;

    //public override bool CanUse(Character user, Vector3 targetPos, Character targetChar)
    //{
    //    if (!base.CanUse(user)) return false;
    //    if (user.CheckFullHealth()) return false;
    //    return true;
    //}

    public override bool Use(Character user, Vector3 targetPos, Character targetChar)
    {
        if (!base.Use(user, targetPos, targetChar)) return false;
        user.SpawnProjectile(projectile, targetChar);
        return true;
    }
}
