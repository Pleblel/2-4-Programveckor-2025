using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    [SerializeField] Item item;
    [SerializeField] InventoryManager IManager;

    bool itemPickedUp = false;


    private void Update()
    {
        if (itemPickedUp)
            gameObject.SetActive(false);
    }


    void Pickup()
    {
        IManager.Add(item);
        IManager.ListItem();
       
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Pickup();
            itemPickedUp = true;
        }
        
    }
}
