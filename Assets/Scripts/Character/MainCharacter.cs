using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : Character
{
    [Header("Mana")]
    [SerializeField] protected int currentMana = 100;
    [SerializeField] protected int maximumMana = 100;
    [SerializeField] protected float manaRegenPerSecond;
    [SerializeField] protected float manaRegenPerFrame;
    [SerializeField] protected float manaRegenRemainder;

    [Header("Inventory")]
    [SerializeField] protected Inventory inventory;

    [Header("Grimoire")]
    [SerializeField] protected Grimoire grimoire;
    
    protected override void Start()
    {
        base.Start();
        inventory = GetComponentInChildren<Inventory>();
    }

    protected override void Update()
    {
        base.Update();
        ManaRegen();
    }

    #region Movement
    public override bool CanMove()
    {
        if (!base.CanMove()) return false;
        bool checkItem = !inventory.IsUsingItem();
        bool checkSpell = !inventory.IsUsingItem();
        return checkItem && checkSpell;
    }
    #endregion

    #region Mana
    public int GetCurrentMana() { return currentMana; }
    public int GetMaximumMana() { return maximumMana; }
    public bool LoseMana(int amount)
    {
        if (amount < 0) amount = 0;
        currentMana -= amount;
        //TODO: will use negative mana?
        //if (currentMana < 0) currentMana = 0;
        return CheckNoMana();
    }
    public bool CheckNoMana() { return currentMana <= 0; }
    public bool GainMana(int amount)
    {
        if (amount < 0) amount = 0;
        currentMana += amount;
        if (currentMana > maximumMana) currentMana = maximumMana;
        return CheckFullMana();
    }
    public bool CheckFullMana() { return currentMana >= maximumMana; }
    private void ManaRegen()
    {
        float baseFactor = maximumMana / 150F;
        float healthFactor = currentHealth / 100F;
        manaRegenPerSecond = baseFactor * healthFactor;

        if (CheckFullMana())
        {
            manaRegenPerFrame = 0;
            manaRegenRemainder = 0;
        }
        else
        {
            manaRegenPerFrame = manaRegenPerSecond * Time.deltaTime;
            manaRegenRemainder += manaRegenPerFrame % 1;
            int actualManaRegen = Mathf.FloorToInt(manaRegenPerFrame);
            if (manaRegenRemainder >= 1)
            {
                actualManaRegen += 1;
                manaRegenRemainder -= 1;
            }
            GainMana(actualManaRegen);
        }
    }
    #endregion

    #region Inventory
    public Inventory GetInventory() { return inventory; }
    public void NextItem() { inventory.NextItem(); }
    public void PreviousItem() { inventory.PreviousItem(); }
    public bool CanUseItem()
    {
        AbstractItem item = inventory.GetSelectedItem();
        bool checkItem = item && !inventory.IsUsingItem();
        return checkItem;
    }
    public void UseItem(Vector3 targetPos, Character targetChar)
    {
        if (!CanUseItem()) return;
        inventory.StartItem(targetPos, targetChar);
    }
    #endregion

    #region Grimoire
    public Grimoire GetGrimoire() { return grimoire; }
    public void NextSpell() { grimoire.NextSpell(); }
    public void PreviousSpell() { grimoire.PreviousSpell(); }
    public bool CanCastSpell()
    {
        AbstractSpell spell = grimoire.GetSelectedSpell();
        bool checkSpell = spell && !grimoire.IsCastingSpell();
        return checkSpell;
    }
    public void CastSpell(Vector3 targetPos, Character targetChar)
    {
        if (!CanCastSpell()) return;
        grimoire.StartSpell(targetPos, targetChar);
    }
    #endregion
}
