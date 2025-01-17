using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool hasElectricity = false; // Set in the Inspector for each room

    [SerializeField] private List<GameObject> lights; // Assign lights in the Inspector

    void Start()
    {
        UpdateLights(); // Ensure the initial state is correct
    }

    private void Update()
    {
        UpdateLights();
      
        
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
