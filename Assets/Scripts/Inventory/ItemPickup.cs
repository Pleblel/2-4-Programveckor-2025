using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    [SerializeField] Item item;
    [SerializeField] InventoryManager IManager;

    void Pickup()
    {
        IManager.Add(item);
        IManager.ListItem();
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        Pickup();
    }
}
