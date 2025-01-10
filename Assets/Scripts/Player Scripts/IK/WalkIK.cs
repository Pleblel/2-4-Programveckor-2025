using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WalkIK : MonoBehaviour
{
    [SerializeField] List<GameObject> feet;
    [SerializeField] List<GameObject> targetPoint;

    [Header("Raycast Settings")]
    public LayerMask groundLayer;       // Layer to identify the ground

    // Update is called once per frame
    void Update()
    {
        MoveTargetPoints();
        LockFeetToGround();
    }

    void MoveTargetPoints()
    {
        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Vertical") == 1)
        {
            foreach(var target in targetPoint)
            {
                target.transform.position = target.transform.position + target.transform.forward;
            }
        }
    }

    void LockFeetToGround()
    {
        foreach (var foot in feet)
        {
            Vector3 rayOrigin = foot.transform.position + Vector3.up;

            // Perform the raycast
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 10, groundLayer))
            {
                foot.transform.position = new Vector3(foot.transform.position.x, (float)(foot.transform.position.y - hit.distance + 1.1743169), foot.transform.position.z);
            }
        }
    }
}

