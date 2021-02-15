using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel_Player : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private UISelectionItem selectedItem;
    [SerializeField] private UISelectionItem selectedSpell;
    [SerializeField] private Image mpFill;
    [SerializeField] private Image hpFill;

    public void ManualUpdate(MainCharacter mainCharacter)
    {
        if (mainCharacter)
        {
            AbstractItem item = mainCharacter.GetInventory().GetSelectedItem();
            selectedItem.Set(item);

            //TODO: this one
            selectedSpell.Set(null);

            float mpFillAmount = 1F * mainCharacter.GetCurrentMana() / mainCharacter.GetMaximumMana();
            mpFill.fillAmount = mpFillAmount;

            float hpFillAmount = 1F * mainCharacter.GetCurrentHealth() / mainCharacter.GetMaximumHealth();
            hpFill.fillAmount = hpFillAmount;
        }
        gameObject.SetActive(mainCharacter);
    }
}
