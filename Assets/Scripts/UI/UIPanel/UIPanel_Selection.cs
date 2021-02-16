using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel_Selection : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private UISelectionBar selectionBar;

    [Header("Settings")]
    [SerializeField] private float duration;
    [SerializeField] private float durationMax = 3F;

    private void Update()
    {
        if (duration > 0) duration -= Time.deltaTime;
        else selectionBar.CloseBar();
    }

    public void ManualUpdate()
    {
        selectionBar.CloseBar();
        gameObject.SetActive(false);
    }

    public void ManualUpdate(List<AbstractItem> itemList, AbstractItem selectedItem)
    {
        bool canShow = selectionBar.OpenBar(itemList, selectedItem);
        duration = durationMax;
        gameObject.SetActive(canShow);
    }

    public void ManualUpdate(List<AbstractSpell> spellList, AbstractSpell selectedSpell)
    {
        bool canShow = selectionBar.OpenBar(spellList, selectedSpell);
        duration = durationMax;
        gameObject.SetActive(canShow);
    }
}
