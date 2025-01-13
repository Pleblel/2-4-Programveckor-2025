using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private Light sceneLight;

    private void Start()
    {
        sceneLight = GetComponent<Light>();

        // Subscribe to the switch event
        SwitchController.OnSwitchActivated += ToggleLight;
    }

    private void ToggleLight()
    {
        if (sceneLight != null)
        {
            sceneLight.enabled = !sceneLight.enabled; // Toggle the light state
            Debug.Log("Light toggled: " + (sceneLight.enabled ? "On" : "Off"));
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        SwitchController.OnSwitchActivated -= ToggleLight;
    }
}