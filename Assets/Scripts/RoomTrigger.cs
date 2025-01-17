using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Room room = GetComponent<Room>();
            if (room != null)
            {
                FindObjectOfType<GameManager>().SetCurrentRoom(room);
            }
        }
    }
}
