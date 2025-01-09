using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float interactionDistance = 1.5f;
    public float interactionSphereSize = 1f;
    public LayerMask movableLayer;
    public Transform playerTransform;
    public float moveSpeed = 5f;

    private GameObject currentObject;
    private bool isMovingObject = false;
    private Vector3 interactionPoint;
    private Vector3 interactVelocity;

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
    }

    private void FixedUpdate()
    {
        interactionPoint = playerTransform.position + playerTransform.forward * interactionDistance;

        if (isMovingObject && currentObject != null)
        {
            MoveObjectWithPlayer();
        }
    }

    void TryGrabObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(interactionPoint, interactionSphereSize, movableLayer);
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
                currentObject.GetComponent<Rigidbody>().useGravity = false;
                isMovingObject = true;
            }
        }
    }

    void MoveObjectWithPlayer()
    {
        currentObject.GetComponent<Rigidbody>().position = Vector3.SmoothDamp(currentObject.transform.position, interactionPoint, ref interactVelocity, 0.2f, Mathf.Infinity, Time.fixedDeltaTime);
        //currentObject.transform.position = interactionPoint;
    }

    void ReleaseObject()
    {
        currentObject.GetComponent<Rigidbody>().useGravity = true;
        currentObject = null;
        isMovingObject = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionPoint, interactionSphereSize);
    }
}