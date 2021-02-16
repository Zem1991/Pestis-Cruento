using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectionBar : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private UISelectionItem prefabUISelectionItem;

    [Header("Self references")]
    [SerializeField] private List<UISelectionItem> selectionItemList = new List<UISelectionItem>();
    
    private void ClearBar()
    {
        foreach (UISelectionItem item in selectionItemList)
        {
            Destroy(item.gameObject);
        }
        selectionItemList.Clear();
    }

    public bool OpenBar(List<AbstractItem> itemList, AbstractItem selectedItem)
    {
        ClearBar();
        if (itemList == null || itemList.Count <= 0) return false;

        foreach (AbstractItem item in itemList)
        {
            UISelectionItem uiItem = Instantiate(prefabUISelectionItem, transform);
            uiItem.Set(item);
            if (item != selectedItem) uiItem.transform.localScale *= 0.75F;
            selectionItemList.Add(uiItem);
        }
        return true;
    }

    public bool OpenBar(List<AbstractSpell> spellList, AbstractSpell selectedSpell)
    {
        ClearBar();
        if (spellList == null || spellList.Count <= 0) return false;

        foreach (AbstractSpell spell in spellList)
        {
            UISelectionItem uiItem = Instantiate(prefabUISelectionItem, transform);
            uiItem.Set(spell);
            if (spell != selectedSpell) uiItem.transform.localScale *= 0.75F;
            selectionItemList.Add(uiItem);
        }
        return true;
    }

    public void CloseBar()
    {
        ClearBar();
    }
}
