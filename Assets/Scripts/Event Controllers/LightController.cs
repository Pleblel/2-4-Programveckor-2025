using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private Light sceneLight;

    private void Start()
    {
        sceneLight = GetComponent<Light>();
        SwitchController.OnSwitchActivated += ToggleLight;
    }

    private void ToggleLight()
    {
        if (sceneLight != null)
        {
            sceneLight.enabled = !sceneLight.enabled;
            Debug.Log("Light toggled: " + (sceneLight.enabled ? "On" : "Off"));
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        SwitchController.OnSwitchActivated -= ToggleLight;
    }
}