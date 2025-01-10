using Cinemachine;
using UnityEngine;

public class CameraLockOn : MonoBehaviour
{
    //Darren


    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera lockOnCamera;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Variables")]
    [SerializeField] private float maxLockOnDistance = 20f;
   
    [Header("Directions")]
    private Transform currentTarget;
    private CameraController mainCameraController;
    private PlayerMovement playerMovement;

    private void Start()
    {
        mainCameraController = Camera.main.GetComponent<CameraController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire2")) // Left ctrl 
        {
            ToggleLockOn();
        }

        if (currentTarget != null)
        {
            if (HasLineOfSight(currentTarget))
                UpdateLockOnCamera();
            else
               DisableLockOn();
        }

      

        
    }

    private void ToggleLockOn()
    {
        if (currentTarget == null)
        {
            FindAndLockOnTarget();
        }
        else
        {
            DisableLockOn();
        }
    }

    private void FindAndLockOnTarget()
    {
        //Find potentialTargets based on the Sphere created
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, maxLockOnDistance, enemyLayer);

        
        float closestAngle = float.MaxValue; //Find the closest angle by setting it to the max value of a float
        Transform closestTarget = null;

        //Search for each targetCollider in potential targets
        foreach (Collider targetCollider in potentialTargets)
        {
            //Find the direction/distance from the enemy to the player
            Vector3 directionToTarget = (targetCollider.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            //Set the closest angle and closestTarget based off of the code above if the target has line of sight
            if (HasLineOfSight(targetCollider.transform) && angle < closestAngle)
            {
                closestAngle = angle; 
                closestTarget = targetCollider.transform;
            }
        }

       
        if (closestTarget != null)
        {
            currentTarget = closestTarget;
            EnableLockOn();
        }
    }

    private bool HasLineOfSight(Transform target)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if(Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, distanceToTarget, obstacleLayer | enemyLayer))
        {
            return hit.transform == target;
        }

        return false;
    }

    

    private void EnableLockOn()
    {
        //Disable and enable scripts
        mainCameraController.ToggleMouseControl(false);
        lockOnCamera.gameObject.SetActive(true);
        playerMovement.SetLockOnTarget(currentTarget);
    }

    private void DisableLockOn()
    {
        //Disable and enable scripts
        mainCameraController.ToggleMouseControl(true);
        lockOnCamera.gameObject.SetActive(false);
        currentTarget = null;
        playerMovement.SetLockOnTarget(null);
    }

    private void UpdateLockOnCamera()
    {
        //Set the camera to face towards the enemy
        Vector3 middlePoint = (transform.position + currentTarget.position) / 2f;
        cameraTarget.position = middlePoint;
    }
}
