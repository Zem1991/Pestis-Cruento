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

    public bool OpenBar(List<AbstractItem> itemList, AbstractItem currentSelection)
    {
        ClearBar();
        if (itemList == null || itemList.Count <= 0) return false;

        foreach (AbstractItem item in itemList)
        {
            UISelectionItem uiItem = Instantiate(prefabUISelectionItem, transform);
            uiItem.Set(item);
            if (item != currentSelection) uiItem.transform.localScale *= 0.75F;
            selectionItemList.Add(uiItem);
        }
        return true;
    }

    public void CloseBar()
    {
        ClearBar();
    }
}
