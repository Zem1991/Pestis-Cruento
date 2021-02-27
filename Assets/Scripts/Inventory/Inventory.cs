using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private MainCharacter _mainCharacter;

    [Header("Settings")]
    [SerializeField] private List<AbstractItem> itemList = new List<AbstractItem>();

    [Header("Execution")]
    [SerializeField] private AbstractItem selectedItem;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private GameObject targetObj;
    [SerializeField] private bool itemStart;
    [SerializeField] private bool itemEnd;
    [SerializeField] private float currentDuration;

    public List<AbstractItem> GetItemList() { return itemList; }
    public bool AddItem(AbstractItem item)
    {
        AbstractItem actualItem = null;
        foreach (AbstractItem abItem in itemList)
        {
            if (abItem.CheckSame(item))
            {
                actualItem = abItem;
                break;
            }
        }
        if (!actualItem) actualItem = Instantiate(item, transform);

        if (!itemList.Contains(actualItem)) itemList.Add(actualItem);
        if (!selectedItem) selectedItem = actualItem;
        return actualItem.IncreaseAmount();
    }

    public AbstractItem GetSelectedItem() { return selectedItem; }

    private void Start()
    {
        _mainCharacter = GetComponentInParent<MainCharacter>();
    }

    private void Update()
    {
        if (!selectedItem) return;
        float startDuration = selectedItem.GetStartDuration();
        float endDuration = selectedItem.GetEndDuration();

        if (itemStart)
        {
            currentDuration += Time.deltaTime;
            if (currentDuration >= startDuration)
            {
                itemStart = false;
                itemEnd = true;
                currentDuration = 0;
                selectedItem.Use(_mainCharacter, targetPos, targetObj);
            }
        }
        else if (itemEnd)
        {
            currentDuration += Time.deltaTime;
            if (currentDuration >= endDuration)
            {
                targetPos = Vector3.zero;
                targetObj = null;
                itemEnd = false;
                currentDuration = 0;
            }
        }
    }

    public void NextItem()
    {
        if (!GetSelectedItem()) return;
        int index = itemList.IndexOf(selectedItem) + 1;
        if (index >= itemList.Count) index = 0;
        selectedItem = itemList[index];
    }

    public void PreviousItem()
    {
        if (!GetSelectedItem()) return;
        int index = itemList.IndexOf(selectedItem) - 1;
        if (index < 0) index = itemList.Count - 1;
        selectedItem = itemList[index];
    }

    public bool StartItem(Vector3 targetPos, GameObject targetObj)
    {
        if (IsUsingItem()) return false;
        if (!selectedItem) return false;

        if (selectedItem.IsUseOnSelf()) targetObj = _mainCharacter.gameObject;
        if (!selectedItem.CanUse(_mainCharacter, targetPos, targetObj)) return false;

        this.targetPos = targetPos;
        this.targetObj = targetObj;
        itemStart = true;
        return true;
    }

    public bool IsUsingItem()
    {
        return itemStart || itemEnd;
    }

    public bool InterruptItem()
    {
        if (!IsUsingItem()) return false;
        targetPos = Vector3.zero;
        targetObj = null;
        itemStart = false;
        itemEnd = false;
        currentDuration = 0;
        return true;
    }
}
