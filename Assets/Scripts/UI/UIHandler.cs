using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private UIPanel_Player player;
    [SerializeField] private UIPanel_Selection selection;

    public void ManualUpdatePlayer(MainCharacter mainCharacter)
    {
        player.ManualUpdate(mainCharacter);
        if (!mainCharacter) selection.ManualUpdate();
    }

    public void ManualUpdateSelection(List<AbstractItem> itemList, AbstractItem selectedItem)
    {
        selection.ManualUpdate(itemList, selectedItem);
    }

    internal void ManualUpdateSelection(List<AbstractSpell> spellList, AbstractSpell selectedSpell)
    {
        selection.ManualUpdate(spellList, selectedSpell);
    }
}
