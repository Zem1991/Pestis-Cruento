using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : AbstractEffect
{
    [Header("Settings")]
    [SerializeField] private int amountMin;
    [SerializeField] private int amountMax;

    public override bool ExecuteEffect(Character caster, Vector3 targetPos, Character targetChar)
    {
        int amount = Random.Range(amountMin, amountMax);
        return targetChar.GainHealth(amount);
    }
}
