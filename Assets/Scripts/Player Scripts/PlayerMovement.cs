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

    [Header("Movement Settings")] 
    float originalMovementSpeed;
    float runningSpeed;
    [SerializeField] float rotationSpeed = 7.0f;

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    private float currentStamina;
    [SerializeField] private float staminaDepletionRate = 20f;
    [SerializeField] private float staminaRegenRate = 15f;
    [SerializeField] private float staminaRegenDelay = 1f;
    [SerializeField] private float minStaminaToRun = 10f;
    private float timeLastUsedStamina;
    private bool isRunning = false;


    public float movementSpeed { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        movementSpeed = 6.0f;
        runningSpeed = movementSpeed * 1.5f;
        originalMovementSpeed = movementSpeed;
        currentStamina = maxStamina;
    }

    void Update()
    {
        diretionX = Input.GetAxisRaw("Horizontal");
        directionZ = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector3(diretionX, 0f, directionZ).normalized;

        HandleRunning();
    }

    private void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;
        Move(movementDirection);
    }

    public void Move(Vector3 direction)
    {
        Vector3 targetVelocity = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * direction * movementSpeed;
        targetVelocity.y = rb.velocity.y;
        rb.velocity = targetVelocity;

        if (lockOnTarget != null)
        {
            Vector3 targetDirection = (lockOnTarget.position - transform.position).normalized;
            targetDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (targetVelocity != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(targetVelocity, Vector3.up);
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
        currentStamina += staminaRegenRate * Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina, maxStamina);
    }

    public void SetLockOnTarget(Transform target)
    {
        lockOnTarget = target;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Vector3 boxCenter = transform.position + transform.forward * 2;
        Gizmos.DrawWireSphere(boxCenter, 0.5f);
    }
}
