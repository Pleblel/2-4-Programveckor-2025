using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject destinationDoor;
    public float exitOffset = 2f;
    GameManager gm;

    private void Update()
    {
        gm = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 exitPosition = destinationDoor.transform.position + destinationDoor.transform.right * exitOffset;
            other.transform.position = new Vector3(exitPosition.x, other.transform.position.y, exitPosition.z);
        }
    }

    private IEnumerator TransitionPlayer(Transform player)
    {
        gm.FadeIn();

        // Freeze the game
        Time.timeScale = 0f;

        // Wait for the specified duration (using unscaled time)
        yield return new WaitForSecondsRealtime(freezeDuration);

        // Move the player to the destination door
        Vector3 exitPosition = destinationDoor.transform.position + destinationDoor.transform.forward * exitOffset;
        player.position = exitPosition;

        // Unfreeze the game
        Time.timeScale = 1f;
    }
}
