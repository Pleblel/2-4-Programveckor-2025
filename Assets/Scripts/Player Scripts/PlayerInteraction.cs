using System.Linq;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float interactionDistance;
    public float moveSpeed;
    public float offset;
    public float movingOffset;
    public LayerMask movableLayer;
    public Transform playerTransform;

    private GameObject currentObject;
    private bool isMovingObject = false;
    private Vector3 interactionPoint;
    private Vector3 boxHalfExtent = new Vector3(0.5f, 1, 0.2f);
    private Transform playerOrientation;
    private Vector3 difference;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isMovingObject)
            {
                TryGrabObject();
            }
            else
            {
                ReleaseObject();
            }
        }

        interactionPoint = playerTransform.position + playerTransform.forward * interactionDistance;

        if (isMovingObject && currentObject != null)
        {
            Vector3 currentRotation = transform.eulerAngles;
            float roundedX = Mathf.Round(currentRotation.x / 90f) * 90f;
            float roundedY = Mathf.Round(currentRotation.y / 90f) * 90f;
            float roundedZ = Mathf.Round(currentRotation.z / 90f) * 90f;
            transform.eulerAngles = new Vector3(roundedX, roundedY, roundedZ);
            MoveObjectWithPlayer();
        }
    }

    void TryGrabObject()
    {
        Collider[] hitColliders = Physics.OverlapBox(interactionPoint, boxHalfExtent, gameObject.transform.rotation, movableLayer);
        if (hitColliders.Length > 0)
        {
            float closestDistance = Mathf.Infinity;
            Collider closestCollider = null;

            foreach (var collider in hitColliders)
            {
                float distance = Vector3.Distance(playerTransform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = collider;
                }
            }

            if (closestCollider != null)
            {
                currentObject = closestCollider.gameObject;
                isMovingObject = true;
                Vector3 currentPosition = transform.position;
                Bounds objectBounds = currentObject.GetComponent<Renderer>().bounds;
                Vector3 rightFaceCenter = new Vector3(objectBounds.max.x, objectBounds.center.y, objectBounds.center.z);
                Vector3 leftFaceCenter = new Vector3(objectBounds.min.x, objectBounds.center.y, objectBounds.center.z);
                Vector3 frontFaceCenter = new Vector3(objectBounds.center.x, objectBounds.center.y, objectBounds.max.z);
                Vector3 backFaceCenter = new Vector3(objectBounds.center.x, objectBounds.center.y, objectBounds.min.z);

                float distanceToRight = Vector3.Distance(currentPosition, rightFaceCenter);
                float distanceToLeft = Vector3.Distance(currentPosition, leftFaceCenter);
                float distanceToFront = Vector3.Distance(currentPosition, frontFaceCenter);
                float distanceToBack = Vector3.Distance(currentPosition, backFaceCenter);

                Debug.Log(objectBounds.min.x);

                float minDistance = Mathf.Min(distanceToRight, distanceToLeft, distanceToFront, distanceToBack);
                Vector3 newPosition = currentPosition;

                if (minDistance == distanceToRight)
                {
                    newPosition.x = objectBounds.max.x + offset;
                    newPosition.z = objectBounds.center.z;
                }
                else if (minDistance == distanceToLeft)
                {
                    newPosition.x = objectBounds.min.x - offset;
                    newPosition.z = objectBounds.center.z;
                }
                else if (minDistance == distanceToFront)
                {
                    newPosition.x = objectBounds.center.x;
                    newPosition.z = objectBounds.max.z + offset;
                }
                else // Back face
                {
                    newPosition.x = objectBounds.center.x;
                    newPosition.z = objectBounds.min.z - offset;
                }

                transform.position = newPosition;

                float newXComponent = currentObject.transform.position.x - gameObject.transform.position.x;
                float newYComponent = currentObject.transform.position.y - gameObject.transform.position.y;
                float newZComponent = currentObject.transform.position.z - gameObject.transform.position.z;
                difference = new Vector3(newXComponent, newYComponent, newZComponent);
            }
        }
    }

    void MoveObjectWithPlayer()
    {
        currentObject.transform.position = transform.position + difference.normalized * movingOffset;

        //currentObject.GetComponent<Rigidbody>().position = Vector3.SmoothDamp(currentObject.transform.position, interactionPoint, ref interactVelocity, 0.2f, Mathf.Infinity, Time.fixedDeltaTime);
        //currentObject.transform.position = interactionPoint;
    }

    void ReleaseObject()
    {
        currentObject = null;
        isMovingObject = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(interactionPoint, boxHalfExtent);
    }
}