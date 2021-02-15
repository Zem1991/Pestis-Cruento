using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : AbstractEffect
{
    [Header("Settings")]
    [SerializeField] private int amountMin;
    [SerializeField] private int amountMax;

    public override bool ExecuteEffect(Character caster, Vector3 targetPos, Character targetChar)
    {
        if (targetChar)
        {
            int amount = Random.Range(amountMin, amountMax);
            return targetChar.LoseHealth(amount);
        }
        return false;
    }
}
