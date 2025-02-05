using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public bool buttonPressed = false;
    [SerializeField] BoxCollider bc;
    public Room room;
    

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider>();
        room = GetComponentInParent<Room>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateElectricityState();
    }

    void UpdateElectricityState()
    {
        if (room != null)  // Ensure room is assigned
        {
            room.hasElectricity = buttonPressed;  // Sync with button state
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Box") || collision.collider.CompareTag("Player") || collision.collider.CompareTag("Enemy"))
            buttonPressed = true;

        if (collision.collider.CompareTag("Room"))
        {
            room = collision.collider.GetComponent<Room>();
        }
    }
    private void OnCollisionExit(Collision collision)
    {

        if (collision.collider.CompareTag("Box") || collision.collider.CompareTag("Player") || collision.collider.CompareTag("Enemy"))
            buttonPressed = false;
    }
}
