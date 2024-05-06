using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGunController : MonoBehaviour
{
    [SerializeField]
    private Transform desiredPos;
    [SerializeField]
    private SpringJoint2D springJoint;
    private bool isGrabbing = false;

    [SerializeField]
    private LineRenderer lineRenderer;

    private Vector3[] linePositions = new Vector3[2];
    void Start()
    {


    }

    void Update()
    {
        //set desiredPos's position to mouse pos
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
        desiredPos.position = new Vector3(worldPos.x, worldPos.y, desiredPos.position.z);

        if (isGrabbing)
        {
            linePositions[0] = transform.position;
            linePositions[1] = desiredPos.position;
            lineRenderer.SetPositions(linePositions);
        }
        lineRenderer.enabled = isGrabbing;


        if (!isGrabbing && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, desiredPos.position - transform.position, Vector2.Distance(transform.position, desiredPos.position), LayerMask.GetMask("Grabbable"));
            if (hit)
            {
                isGrabbing = true;
                Rigidbody2D hitRb = hit.rigidbody;
                //hitRb.gravityScale = 0;
                springJoint.connectedBody = hitRb;
                springJoint.connectedAnchor = hit.point - (Vector2)hit.transform.position;
            }
        }
        else if (isGrabbing && Input.GetMouseButtonUp(0))
        {
            isGrabbing = false;
            if (springJoint.connectedBody != null)
            {
                Rigidbody2D connectedBody = springJoint.connectedBody;
                //connectedBody.gravityScale = 1;
                springJoint.connectedBody = null;
            }
        }
    }
}
