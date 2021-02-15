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

    public void ManualUpdate(List<AbstractItem> itemList, AbstractItem currentSelection)
    {
        bool canShow = selectionBar.OpenBar(itemList, currentSelection);
        duration = durationMax;
        gameObject.SetActive(canShow);
    }
}
