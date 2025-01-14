using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class WalkIK : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform body = default;
    [SerializeField] WalkIK otherFoot = default;
    [SerializeField] float speed = 1;
    [SerializeField] float stepDistance = 4;
    [SerializeField] float stepLength = 4;
    [SerializeField] float stepHeight = 1;
    [SerializeField] Vector3 footOffset = default;
    [SerializeField] Rigidbody bodyRb = null;
    [SerializeField] AnimationCurve stepCurve;
    [SerializeField] float stepOffset;
    [SerializeField] CameraLockOn cameraLockOn;

    [Header("Locked on stepOffset")]
    [SerializeField] float stepOffsetX;
    [SerializeField] float stepOffsetZ;

    float footSpacing;
    Vector3 oldPosition, currentPosition, newPosition;
    float lerp;
    int doOnce = 1;

    private void Start()
    {
        footSpacing = transform.localPosition.x;
        currentPosition = newPosition = oldPosition = transform.position;
        lerp = 1;
    }

    private void Update()
    {
        float directionX = Input.GetAxisRaw("Horizontal");
        float directionZ = Input.GetAxisRaw("Vertical");

        transform.position = currentPosition;

        Ray movingRay;
        if(cameraLockOn.isLockedOn == true) 
        {
            if (directionZ < 0)
            {
                movingRay = new Ray(body.position + (body.right * footSpacing) - (body.transform.forward * Mathf.Abs(directionZ) * 3f * stepOffsetZ) + (body.transform.right * directionX * stepOffsetX * 2), Vector3.down);
            }
            else
            {
                movingRay = new Ray(body.position + (body.right * footSpacing) + (body.transform.forward * directionZ * stepOffsetZ) + (body.transform.right * directionX * stepOffsetX * 2), Vector3.down);
            }
        } 
        else
        {
            movingRay = new Ray(body.position + (body.right * footSpacing) + (body.transform.forward * stepOffset), Vector3.down);
        }
        


        Ray stillRay = new Ray(body.position + (body.right * footSpacing), Vector3.down);

        if (bodyRb.velocity.magnitude > 0.1f)
        {
            HandleMovement(movingRay, true, directionZ);
        }
        else
        {
            HandleMovement(stillRay, false, directionZ);
        }
    }

    private void HandleMovement(Ray ray, bool isMoving, float dirZ)
    {
        if (isMoving)
        {
            doOnce = 1;
            if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
            {
                if (Vector3.Distance(newPosition, info.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
                {
                    lerp = 0;
                    int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                    newPosition = info.point + (body.forward * stepLength * direction) + footOffset;
                }
            }
        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
            {
                if (!otherFoot.IsMoving() && lerp >= 1)
                {
                    if (doOnce == 1)
                    {
                        lerp = 0;
                        doOnce = 0;
                    }
                    newPosition = info.point + footOffset;
                }
            }
        }

        if (lerp < 1)
        {
            float curveValue = stepCurve.Evaluate(lerp);
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, curveValue);
            tempPosition.y += Mathf.Sin(curveValue * Mathf.PI) * stepHeight;

            // Adjust rotation using a switch case for dirZ
            float sineOffset = Mathf.Sin(curveValue * Mathf.PI) * 45;
            Vector3 currentEulerAngles = transform.localEulerAngles;
            switch (Mathf.Sign(dirZ))
            {
                case 1: // Moving forward
                    currentEulerAngles.x = -69.5f + sineOffset;
                    break;
                case -1: // Moving backward
                    currentEulerAngles.x = -69.5f;
                    break;
                case 0: // Neutral (no sine offset)
                    currentEulerAngles.x = -69.5f;
                    break;
            }
            transform.localRotation = Quaternion.Euler(currentEulerAngles);


            currentPosition = tempPosition;
            lerp += Time.deltaTime * speed;

            // Recalculate new position if movement direction changes
            if (isMoving && Physics.Raycast(ray, out RaycastHit movingInfo, 10, terrainLayer.value))
            {
                if (Vector3.Distance(newPosition, movingInfo.point) > stepDistance * 0.5f)
                {
                    newPosition = movingInfo.point + (body.forward * stepLength) + footOffset;
                }
            }
        }
        else
        {
            oldPosition = newPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.5f);
    }

    public bool IsMoving()
    {
        return lerp < 1;
    }
}
