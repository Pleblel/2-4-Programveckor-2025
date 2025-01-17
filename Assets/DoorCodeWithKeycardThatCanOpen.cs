using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCodeWithKeycardThatCanOpen : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer; // Set to "Player" layer
    [SerializeField] InventoryManager inventory;
    [SerializeField] Item Keycard;
    [SerializeField] Transform cool;
    bool isPlayerInside = false;
    bool isOpening = false;
    int timer = 0;

    private void Start()
    {

    }
    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E) && inventory.items.Contains(Keycard))
        {
            if (!isOpening)
            {
                Debug.Log("Hello chuzz");
                StartCoroutine(DoorOpen()); 
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Exited: {other.gameObject.name}, Layer: {other.gameObject.layer}");
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Exited: {other.gameObject.name}, Layer: {other.gameObject.layer}");
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInside = false;
        }
    }

    IEnumerator DoorOpen()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = cool.position;
        Vector3 endPosition = startPosition + new Vector3(0, 5f, 0);

        while (elapsedTime < 2)
        {
            cool.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / 2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cool.position = endPosition;
    }
}
