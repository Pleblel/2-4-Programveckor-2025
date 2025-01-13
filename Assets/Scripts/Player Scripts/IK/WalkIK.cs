using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WalkIK : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform body = default;
    [SerializeField] WalkIK otherFoot = default;
    [SerializeField] float speed = 1;
    [SerializeField] float stepDistance = 4;
    [SerializeField] float stepLength = 4;
    [SerializeField] float stepHeight = 1;
    [SerializeField] float stepOffset;
    [SerializeField] Vector3 footOffset = default;
    [SerializeField] Rigidbody bodyRb = null;
    [SerializeField] AnimationCurve stepCurve;

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
        transform.position = currentPosition;

        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);

        if (bodyRb.velocity.magnitude > 0.1f)
        {
            HandleMovement(ray, true);
        }
        else
        {
            HandleMovement(ray, false);
        }
    }

    private void HandleMovement(Ray ray, bool isMoving)
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
                    newPosition = info.point + (body.forward * stepLength * direction) + footOffset + body.transform.forward * stepOffset;
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

            currentPosition = tempPosition;
            lerp += Time.deltaTime * speed;

            // Recalculate new position if movement direction changes
            if (isMoving && Physics.Raycast(ray, out RaycastHit dynamicInfo, 10, terrainLayer.value))
            {
                if (Vector3.Distance(newPosition, dynamicInfo.point) > stepDistance * 0.5f)
                {
                    newPosition = dynamicInfo.point + (body.forward * stepLength) + footOffset;
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
