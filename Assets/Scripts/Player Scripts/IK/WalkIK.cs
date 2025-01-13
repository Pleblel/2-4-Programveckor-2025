using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WalkIK : MonoBehaviour
{
    /*    [SerializeField] List<GameObject> feet;
        [SerializeField] List<GameObject> targetPoint;
        [SerializeField] Rigidbody rb;

        [Header("Raycast Settings")]
        public LayerMask groundLayer;

        enum PlayerState
        {
            Stopped,
            Moving
        }
        PlayerState currentState = PlayerState.Stopped;

        void Update()
        {
            MoveTargetPoints();
            LockFeetToGround();
        }

        void MoveTargetPoints()
        {
            // Detect player movement based on Rigidbody velocity
            bool isMoving = rb.velocity.magnitude > 0.1f;

            // Switch between player states
            switch (currentState)
            {
                case PlayerState.Stopped:
                    if (isMoving)
                    {
                        currentState = PlayerState.Moving;
                        foreach (var target in targetPoint)
                        {
                            target.transform.position += target.transform.forward; // Move target forward
                        }
                    }
                    break;

                case PlayerState.Moving:
                    if (!isMoving)
                    {
                        currentState = PlayerState.Stopped;
                        foreach (var target in targetPoint)
                        {
                            target.transform.position = target.transform.position - target.transform.forward;
                        }
                    }
                    break;
            } 
        }


        void LockFeetToGround()
        {
            foreach (var foot in feet)
            {
                Vector3 rayOrigin = foot.transform.position + Vector3.up;

                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 10, groundLayer))
                {
                    foot.transform.position = new Vector3(foot.transform.position.x, (float)(foot.transform.position.y - hit.distance + 1.1743169), foot.transform.position.z);
                }
            }
        } */


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

    private float footSpacing;
    private Vector3 oldPosition, currentPosition, newPosition;
    private float lerp;

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
            if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
            {
                if (Vector3.Distance(newPosition, info.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
                {
                    lerp = 0;
                    int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                    newPosition = info.point + (body.forward * stepLength * direction) + footOffset + body.transform.forward * 3f;
                }
            }
        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
            {
                //Ensure the foot only moves if the other foot has finished moving
                if (!otherFoot.IsMoving() && lerp >= 1)
                {
                    lerp = 0;
                    newPosition = info.point;
                }
            }
        }

        if (lerp < 1)
        {
            //Evaluate the AnimationCurve based on lerp progress
            float curveValue = stepCurve.Evaluate(lerp);

            //Apply the curve value to the lerp
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, curveValue);
            tempPosition.y += Mathf.Sin(curveValue * Mathf.PI) * stepHeight;

            currentPosition = tempPosition;
            lerp += Time.deltaTime * speed;
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


