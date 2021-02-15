using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : Character
{
    [Header("Mana")]
    [SerializeField] private int currentMana = 100;
    [SerializeField] private int maximumMana = 100;

    [Header("Inventory")]
    [SerializeField] private Inventory inventory;

    public Inventory GetInventory() { return inventory; }

    protected override void Start()
    {
        base.Start();
        inventory = GetComponentInChildren<Inventory>();
    }

    #region Movement
    public override bool CanMove()
    {
        if (!base.CanMove()) return false;
        bool checkItem = !inventory.IsUsingItem();
        return checkItem;
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
    #endregion

    #region Inventory
    public void NextItem() { inventory.NextItem(); }
    public void PreviousItem() { inventory.PreviousItem(); }
    public bool CanUseItem()
    {
        AbstractItem item = inventory.GetSelectedItem();
        bool checkAttack = item && !inventory.IsUsingItem();
        return checkAttack;
    }
    public void UseItem(Vector3 targetPos, Character targetChar)
    {
        if (!CanUseItem()) return;
        inventory.StartItem(targetPos, targetChar);
    }
    #endregion
}
