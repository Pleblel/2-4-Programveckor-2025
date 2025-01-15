using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public bool buttonPressed = false;
    [SerializeField] BoxCollider bc; 


    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        DisableCollider();
    }

    void DisableCollider()
    {
        if (buttonPressed)
            bc.enabled = false;
    }
}
