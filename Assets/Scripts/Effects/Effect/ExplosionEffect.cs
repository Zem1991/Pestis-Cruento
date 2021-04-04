using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : AbstractEffect
{
    [Header("Settings")]
    [SerializeField] private float strength;
    [SerializeField] private float radius;
    [SerializeField] private float upwardsModifier;
    [SerializeField] private int damage;

    public override bool ExecuteEffect(Character caster, Vector3 targetPos, GameObject targetObj)
    {
        Vector3 explosionCenter = transform.position;

        Collider[] colliders = Physics.OverlapSphere(targetPos, radius);
        foreach (Collider forColl in colliders)
        {
            //Ignore self.
            if (forColl.gameObject == gameObject) continue;

            Vector3 targetCenter = forColl.transform.position;
            Character forChar = forColl.GetComponent<Character>();
            Rigidbody forRB = forColl.GetComponent<Rigidbody>();

            if (forChar)
            {
                targetCenter = forChar.GetTargetablePosition();
                forChar.ReceiveImpact(strength, explosionCenter, radius);
            }
            else if (forRB)
            {
                forRB.AddExplosionForce(strength, explosionCenter, radius, upwardsModifier, ForceMode.Impulse);
            }

            //Ignore the rest if damage means nothing.
            if (!forChar) continue;

            float dist = Vector3.Distance(targetPos, targetCenter);
            float ratio = Mathf.InverseLerp(radius, 0, dist);
            int impactDamage = (int)(ratio * damage);
            forChar.LoseHealth(impactDamage);
        }
        return true;
    }
}
