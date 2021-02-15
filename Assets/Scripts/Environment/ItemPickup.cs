using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AbstractItem item;

    private void OnTriggerEnter(Collider other)
    {
        MainCharacter mainCharacter = other.GetComponent<MainCharacter>();
        if (!mainCharacter) return;

        bool itemAdded = mainCharacter.GetInventory().AddItem(item);
        if (itemAdded) Destroy(gameObject);
    }
}
