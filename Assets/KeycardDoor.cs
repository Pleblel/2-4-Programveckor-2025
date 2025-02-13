using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorCodeWithKeycardThatCanOpen : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer; // Set to "Player" layer
    [SerializeField] InventoryManager inventory;
    [SerializeField] Item Keycard;
    [SerializeField] Transform transform;
    bool isPlayerInside = false;
    bool isOpening = false;
    int timer = 0;

    private void Start()
    {

    }
    private void Update()
    {
        if (isPlayerInside && inventory.items.Contains(Keycard))
        {
            if (!isOpening)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
            isPlayerInside = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInside = false; // Reset flag indicating player left the area
        }
    }

    IEnumerator DoorOpen()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, 5f, 0);

        // Move the door up over 2 seconds
        while (elapsedTime < 2)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / 2); // Smoothly change position
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }
}
