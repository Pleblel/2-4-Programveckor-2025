using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool hasElectricity = false; // Set in the Inspector for each room

    [SerializeField] private List<GameObject> lights; // Assign lights in the Inspector

    public GameObject noElectricityDoor;

    DoorScript dS;

    void Start()
    {
        UpdateLights(); // Ensure the initial state is correct
        dS = noElectricityDoor.GetComponent<DoorScript>();
    }

    private void Update()
    {
        UpdateLights();

        if (!hasElectricity)
            dS.enabled = true;
        else
            dS.enabled = false;

    }

    private void UpdateLights()
    {
        foreach (GameObject light in lights)
        {
            if (light != null)
            {
                light.SetActive(hasElectricity);
            }
        }
    }
}
