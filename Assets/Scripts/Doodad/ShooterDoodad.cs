using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterDoodad : Doodad
{
    [Header("Shooter")]
    [SerializeField] protected Projectile projectile;
    [SerializeField] protected float projSpawnHeight = 1.5F;
    [SerializeField] protected float projSpawnDistance = 0.5F;
    [SerializeField] protected float shootDelayMax = 1F;
    [SerializeField] protected float shootDelayCurrent;

    private void Update()
    {
        shootDelayCurrent += Time.deltaTime;
        if (shootDelayCurrent >= shootDelayMax)
        {
            shootDelayCurrent -= shootDelayMax;
            SpawnProjectile(projectile, null);
        }
    }

    public void SpawnProjectile(Projectile projectile, GameObject targetObj)
    {
        Vector3 pos = transform.position;
        pos += transform.forward * projSpawnDistance;
        pos.y += projSpawnHeight;
        Quaternion rot = transform.rotation;
        Projectile proj = Instantiate(projectile, pos, rot);
        proj.Initialize(gameObject, targetObj);
    }
}
