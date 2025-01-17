using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCodeWithKeycardThatCanOpen : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer; // Set to "Player" layer
    [SerializeField] InventoryManager inventory;
    [SerializeField] Item Keycard;
    GameObject cool;
    bool isPlayerInside = false;
    bool isOpening = false;
    int timer = 0;

    private void Start()
    {
        cool = GetComponentInParent<GameObject>();
    }
    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpening)
            {
                Debug.Log("Hello chuzz");
                DoorOpen(); 
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
 
            isPlayerInside = true;

    }

    private void OnTriggerExit(Collider other)
    {
            isPlayerInside = false;
 
    }

    void DoorOpen()
    {
        timer++;
        cool.transform.position = new Vector3(0, timer * Time.deltaTime, 0);
    }
}
