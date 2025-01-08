using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f;
    public LayerMask movableLayer;
    public Transform playerTransform;
    public float moveSpeed = 5f;

    private GameObject currentObject;
    private bool isMovingObject = false;
    private Vector3 offset;

    private void Awake()
    {
        playerTransform = gameObject.transform;
    }

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

        if (isMovingObject && currentObject != null)
        {
            MoveObjectWithPlayer();
        }
    }

    void TryGrabObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, interactionRange, movableLayer);
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
                LatchOntoObject();
                isMovingObject = true;
            }
        }
    }

    void LatchOntoObject()
    {
        Bounds bounds = currentObject.GetComponent<Collider>().bounds;
        Vector3 closestPoint = bounds.ClosestPoint(playerTransform.position);
        offset = currentObject.transform.position - closestPoint;
    }

    void MoveObjectWithPlayer()
    {
        Vector3 targetPosition = playerTransform.position + offset;
        currentObject.transform.position = Vector3.Lerp(currentObject.transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    void ReleaseObject()
    {
        currentObject = null;
        isMovingObject = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerTransform.position, interactionRange);
    }
}
