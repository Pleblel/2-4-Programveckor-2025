using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool hasElectricity = false; // Set in the Inspector for each room

    [SerializeField] private List<GameObject> lights; // Assign lights in the Inspector

    public GameObject noElectricityDoor;

    public GameObject electricityDoor;


    public GameObject openDoor;

    public GameObject closedDoor;

    DoorScript dS;

    void Start()
    {
        UpdateLights(); // Ensure the initial state is correct

        if(noElectricityDoor != null)
        dS = noElectricityDoor.GetComponent<DoorScript>();

        if (electricityDoor != null)
            dS = electricityDoor.GetComponent<DoorScript>();
    }

    private void Update()
    {
      UpdateLights();

     if(noElectricityDoor != null)
     {
            if (!hasElectricity)
            {
                dS.enabled = true;
                openDoor.SetActive(true);
                closedDoor.SetActive(false);
            }
            else
            {
                dS.enabled = false;
                openDoor.SetActive(false);
                closedDoor.SetActive(true);
            }  
          
                
     }
     
     if(electricityDoor != null)
     {
            if (hasElectricity)
            {
                dS.enabled = true;
                openDoor.SetActive(true);
                closedDoor.SetActive(false);
            }
            else
            {
                dS.enabled = false;
                openDoor.SetActive(false);
                closedDoor.SetActive(true);
            }

        }


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
