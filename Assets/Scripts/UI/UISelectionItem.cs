using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectionItem : MonoBehaviour
{
    [Header("Self references")]
    [SerializeField] private Image background;
    [SerializeField] private Image itemBack;
    [SerializeField] private Image itemImage;
    [SerializeField] private Text amount;

    public void Set(AbstractItem item)
    {
        if (item)
        {
            itemImage.sprite = item.GetItemSprite();
            itemImage.color = Color.white;
            amount.text = item.GetCurrentAmount().ToString();
        }
        else
        {
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            amount.text = null;
        }
    }
}
