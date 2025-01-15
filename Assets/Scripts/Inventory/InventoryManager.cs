using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] static InventoryManager instance;
    [SerializeField] List<Item> items = new List<Item>();

    [SerializeField] Transform itemContent;
    [SerializeField] GameObject inventoryItem;
    private void Awake()
    {
        instance = this;
    }

    public void Add(Item item)
    {
        items.Add(item);
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            ListItem(); // Update UI after removing the item
        }
    }

    public void ListItem()
    {
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in items)
        {
            GameObject obj = Instantiate(inventoryItem, itemContent);
            Debug.Log("Instantiate code passed");
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }
}
