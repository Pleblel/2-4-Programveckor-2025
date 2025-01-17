using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityHandler : MonoBehaviour
{

    public bool electricityOn = false;
    private Room currentRoom; // Tracks the player's current room

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentRoom != null)
        electricityOn = currentRoom.hasElectricity;
    }

    public void SetCurrentRoom(Room room)
    {
        if (room != null)
        {
            currentRoom = room;
            electricityOn = currentRoom.hasElectricity;
            Debug.Log("Electricity is now: " + electricityOn);
        }
    }
}
