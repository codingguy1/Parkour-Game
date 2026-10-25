using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    public Transform GunTip, Camera, Player;
    //Allows you to set the layer that this script can interact with in the editor
    public  LayerMask Grapplable;
    private Vector3 GrapplingPoint;
    private SpringJoint GrappleJoint;

    public float MaxGrapplingDistance = 100f;
    public float maxRopeLength;
    public float minRopeLength;
    public Vector3 currentPosition;
    private LineRenderer Rope; 

    private void Awake()
    {
        //gets the line renderer component automaticlly. Helps with respawning
        Rope = GetComponent<LineRenderer>();

    }
    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Grapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }
    private void LateUpdate()
    {
        DrawRope();
    }
    void Grapple()
    {
        // if a ray cast from the camera position pointed forwards from the camera's perspective hits something
        //it stores the hit object in a variable and returns true
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit HitObject, MaxGrapplingDistance, Grapplable))
        {
            GrapplingPoint = HitObject.point;
            // If player is too far away from the grapple point, do nothing
            if (Vector3.Distance(Player.position, GrapplingPoint) > 200)
            {
                return;
            }
            // creates a spring joint between player and grapple point.
            GrappleJoint = Player.gameObject.AddComponent<SpringJoint>();
            // Auto configuring the anchor breaks it, so it is done manually
            GrappleJoint.autoConfigureConnectedAnchor = false;
            GrappleJoint.connectedAnchor = GrapplingPoint;

            float distanceFromPoint = Vector3.Distance(Player.position, GrapplingPoint);

            //The distance the joint will try to keep from grapple point. 
            // Max distance has to be multiplied by less than 1 so it pulls you towards grappling point
            GrappleJoint.maxDistance = distanceFromPoint * 0.78f;
            //min distance has to be multiplied by a number smaller than maxDistance's mulitplier
            GrappleJoint.minDistance = distanceFromPoint * 0.3f;

            //Amount of force pulling you to the point
            GrappleJoint.spring = 4.5f;
            // amount of counter force against spring force.
            GrappleJoint.damper = 7f;

            GrappleJoint.massScale = 4.5f;

            Rope.positionCount = 2;
            currentPosition = GunTip.position;


        }
    }
    void StopGrapple()
    {
        // Deletes the grapple point
        Rope.positionCount = 0;
        Destroy(GrappleJoint);
    }
    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!GrappleJoint) return;
        //linearly scales current position to grappling point.
        currentPosition = Vector3.Lerp(currentPosition, GrapplingPoint, Time.deltaTime * 8f);
        // Sets the 2 points of the rope
        Rope.SetPosition(0, GunTip.position);
        Rope.SetPosition(1, currentPosition);
    }
}
