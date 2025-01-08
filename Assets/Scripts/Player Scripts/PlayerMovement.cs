using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovable
{

    //Darren

    [Header("PlayerDirections")]
    float diretionX;
    float directionZ;
    Vector3 movementDirection;

    [Header("References")]
    Rigidbody rb;
    [SerializeField] Camera followCamera;
    private Transform lockOnTarget;

    [Header("PlayerVariables")]
    [SerializeField] float dashPower = 5.0f;
    [SerializeField] float dashTime = 0.3f;
    [SerializeField] float dashCoolDown = 1.0f;
    [SerializeField] float rotationSpeed = 7.0f;
    bool canDash = true;
    bool isDashing = false;



    public float movementSpeed { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        movementSpeed = 6.0f;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Get the movement direction inputs
        diretionX = Input.GetAxisRaw("Horizontal");
        directionZ = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector3(diretionX, 0f, directionZ).normalized;
      

        //Returns the line of code, not letting anything under it run
        if (isDashing)
            return;

        DashInput();
    }

    private void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;

        if (isDashing)
            return;
        Move(movementDirection);
    }


    public void Move(Vector3 direction)
    {
        //Set the moveDirection depending on where the camera is facing
        Vector3 targetVelocity = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * direction * movementSpeed;
        targetVelocity.y = rb.velocity.y;
        rb.velocity = targetVelocity;

        if (lockOnTarget != null)
        {
            //Makes the player move around the target it is locked onto
            Vector3 targetDirection = (lockOnTarget.position - transform.position).normalized;
            targetDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (targetVelocity != Vector3.zero)
        {
            //Rotate the player in the direction it is moving
            Quaternion desiredRotation = Quaternion.LookRotation(targetVelocity, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
    }


    private void DashInput()
    {
        //"Fire3" is the left shift keybind
        if (Input.GetButtonDown("Fire3") && canDash)
        {
            StartCoroutine("Dash");
        }
    }


    private IEnumerator Dash()
    {
        //Dashes the player in the direction they are moving
         canDash = false;
         isDashing = true;
         Vector3 dashDirection = movementDirection != Vector3.zero ? movementDirection : transform.forward; //Checks whether to simply dash forward or dash in the movement direction
         rb.velocity = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * dashDirection * dashPower; //Adjusts the dash direction based off of where the camera is facing
         yield return new WaitForSeconds(dashTime);
         rb.velocity = Vector3.zero;
         isDashing = false;
         yield return new WaitForSeconds(dashCoolDown);
         canDash = true;

           
    }

    public void SetLockOnTarget(Transform target)
    {
        lockOnTarget = target;
    }
}
