using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Darren

    [Header("Variables")]
    [SerializeField] float mouseSens = 3.0f;
    [SerializeField] float distanceFromTarget = 6.0f;
    [SerializeField] float smoothTime = 0.2f;
    [SerializeField] Vector2 rotationXMinMax = new Vector2(-40, 40);
    private bool isMouseControlEnabled = true;
    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;



    [Header("Directions")]
    private float rotationY;
    private float rotationX;
    [SerializeField] Transform target;


    // Update is called once per frame
    void Update()
    {
        if(!isMouseControlEnabled) return; //If the player is locked onto an enemy, dont allow for camera movement

        //Move the mouse depending one the speed and axises of the mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSens;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens;

        rotationY += mouseX;

        rotationX = Mathf.Clamp(rotationX, rotationXMinMax.x, rotationXMinMax.y);

        Vector3 _nextRotation = new Vector3(rotationX, rotationY);

        currentRotation = Vector3.SmoothDamp(currentRotation, _nextRotation, ref smoothVelocity, smoothTime);
        transform.localEulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distanceFromTarget;

    }

    public void ToggleMouseControl(bool enable)
    {
        isMouseControlEnabled = enable;
    }

}
