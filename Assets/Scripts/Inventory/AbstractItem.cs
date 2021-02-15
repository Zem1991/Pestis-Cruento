using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractItem : MonoBehaviour
{
    [Header("Identification")]
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemSprite;

    [Header("Amount")]
    [SerializeField] private int currentAmount;

    [Header("Settings")]
    [SerializeField] private float startDuration = 0.35F;
    [SerializeField] private float endDuration = 0.15F;

    #region Identification
    public string GetItemName() { return itemName; }
    public Sprite GetItemSprite() { return itemSprite; }
    #endregion

    #region Amount
    public int GetCurrentAmount() { return currentAmount; }
    public bool CanDecreaseAmount() { return currentAmount > 0; }
    public bool DecreaseAmount()
    {
        if (CanDecreaseAmount())
        {
            currentAmount--;
            return true;
        }
        return false;
    }
    public bool IncreaseAmount()
    {
        if (currentAmount < 99)
        {
            currentAmount++;
            return true;
        }
        return false;
    }
    #endregion

    #region Settings
    public float GetStartDuration() { return startDuration; }
    public float GetEndDuration() { return endDuration; }
    #endregion

    public bool CheckSame(AbstractItem other)
    {
        //Type t1 = GetType();
        //Type t2 = other.GetType();
        string t1 = other.GetItemName();
        string t2 = GetItemName();
        return t1 == t2;
    }

    public virtual bool CanUse(Character user, Vector3 targetPos, Character targetChar)
    {
        if (!CanDecreaseAmount()) return false;
        return true;
    }

    public virtual bool Use(Character user, Vector3 targetPos, Character targetChar)
    {
        if (!DecreaseAmount()) return false;
        return true;
    }
}
