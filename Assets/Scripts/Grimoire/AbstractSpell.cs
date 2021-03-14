using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSpell : MonoBehaviour
{
    [Header("Identification")]
    [SerializeField] private string spellName;
    [SerializeField] private Sprite spellSprite;

    [Header("Search")]
    [SerializeField] private EffectSearch effectSearch;

    [Header("Casting")]
    [SerializeField] private bool castOnSelf;
    [SerializeField] private int manaCost;

    [Header("Settings")]
    [SerializeField] private float startDuration = 0.35F;
    [SerializeField] private float endDuration = 0.15F;

    #region Identification
    public string GetSpellName() { return spellName; }
    public Sprite GetSpellSprite() { return spellSprite; }
    #endregion

    #region Casting
    public bool IsCastOnSelf() { return castOnSelf; }
    public int GetManaCost() { return manaCost; }
    public bool CanPayManaCost(MainCharacter caster) { return manaCost <= caster.GetCurrentMana(); }
    public bool PayManaCost(MainCharacter caster)
    {
        if (CanPayManaCost(caster))
        {
            caster.LoseMana(manaCost);
            return true;
        }
        return false;
    }
    #endregion

    #region Settings
    public float GetStartDuration() { return startDuration; }
    public float GetEndDuration() { return endDuration; }
    #endregion

    public bool CheckSame(AbstractSpell other)
    {
        string t1 = other.GetSpellName();
        string t2 = GetSpellName();
        return t1 == t2;
    }

    public virtual bool CanCast(MainCharacter caster, Vector3 targetPos, GameObject targetObj)
    {
        if (!CanPayManaCost(caster)) return false;
        if (effectSearch && !effectSearch.Search(caster, targetPos, targetObj)) return false;
        return true;
    }

    public virtual bool Cast(MainCharacter caster, Vector3 targetPos, GameObject targetObj)
    {
        if (!PayManaCost(caster)) return false;
        return true;
    }
}
