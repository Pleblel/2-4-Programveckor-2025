using UnityEngine;

public class RoomTrigger : MonoBehaviour
{

    public Room room;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            room = GetComponent<Room>();
            if (room != null)
            {
                FindObjectOfType<ElectricityHandler>().SetCurrentRoom(room);
            }
        }
    }
}
