using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovable
{

    //Darren

    [Header("PlayerDirections")]
    float directionX;
    float directionZ;
    Vector3 movementDirection;

    [Header("References")]
    Rigidbody rb;
    [SerializeField] Camera followCamera;
    private Transform lockOnTarget;
    PlayerGrab PG;

    [Header("Movement Settings")] 
    public float originalMovementSpeed;
    float runningSpeed;
    float slowedSpeed;
    [SerializeField] float runSpeedMultiplier = 1.5f;
    [SerializeField] float impairmentDivider = 2.0f;
    [SerializeField] float rotationSpeed = 7.0f;
    bool beingSlowed;
    bool electrified;
    public bool isBeingKnockedBack = false;

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    private float currentStamina;
    [SerializeField] private float staminaDepletionRate = 20f;
    [SerializeField] private float staminaRegenRate = 15f;
    [SerializeField] private float staminaRegenDelay = 1f;
    [SerializeField] private float minStaminaToRun = 10f;
    private float timeLastUsedStamina;
    private bool isRunning = false;


    public float movementSpeed { get; set; }

    private void Awake()
    {

        rb = GetComponent<Rigidbody>();
        PG = GetComponentInChildren<PlayerGrab>();
        SetMovementSpeed(4.0f);
        originalMovementSpeed = movementSpeed;
        currentStamina = maxStamina;
    }

    void Update()
    {
        directionX = Input.GetAxisRaw("Horizontal");
        directionZ = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector3(directionX, 0f, directionZ).normalized;

        HandleRunning();
        HandleSlow();

    }

    private void FixedUpdate()
    {
        if (isBeingKnockedBack) return;

        rb.angularVelocity = Vector3.zero;
        Move(movementDirection);

    }

    public void Move(Vector3 direction)
    {
        //Make the movement dependent on how the camera is facing on rotation
        Vector3 targetVelocity = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * direction * movementSpeed;
        targetVelocity.y = rb.velocity.y;
        rb.velocity = targetVelocity;

        if (lockOnTarget != null)
        {
            //Make sure the player is constantly facing towards the locked on target and moves around it
            Vector3 targetDirection = (lockOnTarget.position - transform.position).normalized;
            targetDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (targetVelocity != Vector3.zero && !PG.isMovingObject)
        {
            //Stops rotation from being forced on the enemy
           Quaternion desiredRotation = Quaternion.LookRotation(targetVelocity, Vector3.up);
            desiredRotation.x = 0;
           transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }

    }

    private void HandleRunning()
    {
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift);

        if (wantsToRun && CanRun())
        {

            StartRunning();
        }
        else
        {
            StopRunning();
        }

        if (!isRunning && Time.time > timeLastUsedStamina + staminaRegenDelay)
        {
            RegenerateStamina();
        }
    }

    private bool CanRun()
    {
        return currentStamina > minStaminaToRun && movementDirection.magnitude > 0;
    }

    private void StartRunning()
    {
        isRunning = true;
        currentStamina -= staminaDepletionRate * Time.deltaTime;
        timeLastUsedStamina = Time.time;
        currentStamina = Mathf.Max(0, currentStamina);
        movementSpeed = runningSpeed;
    }

    private void StopRunning()
    {
        isRunning = false;
        movementSpeed = originalMovementSpeed;
    }

    private void RegenerateStamina()
    {
        //Regen the stamina based on the regen rate and frames
        currentStamina += staminaRegenRate * Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina, maxStamina);
    }

    public void SetLockOnTarget(Transform target)
    {
        lockOnTarget = target;
    }

    public void SetMovementSpeed(float newMovementSpeed)
    {
        //Handle all movement speed options
        movementSpeed = newMovementSpeed;
        runningSpeed = movementSpeed * runSpeedMultiplier;
        slowedSpeed = movementSpeed / impairmentDivider;
    }

    void HandleSlow()
    {
        //Make the move speed slow
        if (beingSlowed)
        {
            movementSpeed = slowedSpeed;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
     
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Slow"))
        {
            Debug.Log("Hitting slow");
            beingSlowed = true;
        }
        else
        {
            beingSlowed = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slow"))
        {
            Debug.Log("Hitting slow");
            beingSlowed = false;
            movementSpeed = originalMovementSpeed;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Vector3 boxCenter = transform.position + transform.forward * 2;
        Gizmos.DrawWireSphere(boxCenter, 0.5f);
    }
}
