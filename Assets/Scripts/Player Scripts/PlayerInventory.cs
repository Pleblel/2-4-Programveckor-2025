using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    private List<Item> items = new List<Item>();
    
    private int maxSlots = 4;

    public delegate void OnInventoryUpdated();
    public event OnInventoryUpdated InventoryUpdated;

    public void AddItem(Item item)
    {
        if (items.Count >= maxSlots)
        {
            Debug.Log("Inventory is full!");
            return;
        }

        items.Add(item);
        Debug.Log($"Added item: {item.itemName}");
        InventoryUpdated?.Invoke();
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"Removed item: {item.itemName}");
            InventoryUpdated?.Invoke();
        }
        else
        {
            Debug.Log("Item not found in inventory!");
        }
    }

    public void UseItem(Item item)
    {
        if (items.Contains(item))
        {
            item.Use();
            if (!item.isStackable)
            {
                RemoveItem(item);
            }
        }
        else
        {
            Debug.Log("Item not in inventory!");
        }
    }
}