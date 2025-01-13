using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Elm
public class SwitchController : MonoBehaviour
{
    // Define an event to be triggered
    public delegate void SwitchActivatedHandler();
    public static event SwitchActivatedHandler OnSwitchActivated;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ActivateSwitch();
        }
    }

    // This function simulates activating the switch
    public void ActivateSwitch()
    {
        Debug.Log("Switch activated!");
        OnSwitchActivated?.Invoke(); // Trigger the event if there are subscribers
    }
}