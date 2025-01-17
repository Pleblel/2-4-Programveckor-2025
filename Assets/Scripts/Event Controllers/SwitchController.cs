using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Elm
public class SwitchController : MonoBehaviour
{
    public delegate void SwitchActivatedHandler();
    public static event SwitchActivatedHandler OnSwitchActivated;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ActivateSwitch();
        }
    }


    public void ActivateSwitch()
    {
        Debug.Log("Switch activated!");
        OnSwitchActivated?.Invoke();
    }
}