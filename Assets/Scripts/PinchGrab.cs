using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PinchGrab : MonoBehaviour
{
    // This script should be attached to both controller objects in the scene
    // Make sure to define the input in the editor (LeftHand/Grip and RightHand/Grip recommended respectively)
    PinchGrab otherHand = null;
    public List<Transform> nearObjects = new List<Transform>();
    public List<Transform> plugs = new List<Transform>();
    public Transform grabbedObject = null;
    public InputActionReference action;
    bool grabbing = false;

    private Rigidbody grabbedRigidbody;
    private bool gotLastPosition = false;
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    private void Start()
    {
        action.action.Enable();

        // Find the other hand
        foreach(PinchGrab c in transform.parent.parent.GetComponentsInChildren<PinchGrab>())
        {
            if (c != this)
                otherHand = c;
        }
    }

    void Update()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        grabbing = action.action.IsPressed();
        if (grabbing)
        {
            // Grab nearby object or the object in the other hand
            if (!grabbedObject)
                grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : otherHand.grabbedObject;

            if (grabbedObject)
            {
                // Change these to add the delta position and rotation instead
                // Save the position and rotation at the end of Update function, so you can compare previous pos/rot to current here

                //Set the rigidbody of the grabbed object kinematic
                if (grabbedRigidbody == null) {
                    grabbedRigidbody = grabbedObject.GetComponent<Rigidbody>();
                    grabbedRigidbody.isKinematic = true;
                    if (grabbedObject.name.Contains("Connector")) {
                        Debug.Log("Disconnecting Plug");
                        grabbedObject.transform.parent = null;
                        grabbedObject.SendMessage("Disconnected");
                    }
                }
                if (gotLastPosition) {
                    //Calculate position and rotation deltas
                    Quaternion deltaRotation = rotation * Quaternion.Inverse(lastRotation);
                    Vector3 deltaTranslation = position - lastPosition;

                    //Calculate position delta caused by rotation
                    Vector3 relativePos = grabbedObject.position - transform.position;
                    Vector3 newLocalPosition = (deltaRotation * relativePos) - relativePos;

                    //Apply position and position delta
                    grabbedObject.rotation = deltaRotation * grabbedObject.rotation;
                    grabbedObject.position += deltaTranslation + newLocalPosition;
                }
            }
        }
        // If let go of button, release object
        else if (grabbedObject) {
            if (grabbedRigidbody && otherHand.grabbedObject != grabbedObject) {
                //Connect plug when released
                if (plugs.Count > 0 && plugs[0].transform.childCount <= 0) {
                    grabbedObject.transform.position = plugs[0].transform.position;
                    grabbedObject.transform.rotation = plugs[0].transform.rotation;
                    grabbedObject.transform.parent = plugs[0].transform;
                    grabbedObject.SendMessage("Connected");
                } else {
                    grabbedRigidbody.isKinematic = false;
                    if (grabbedObject.parent) {
                        grabbedObject.parent.SendMessage("Reset");
                    }
                }
                grabbedRigidbody = null;

            }
            grabbedObject = null;
            gotLastPosition = false;
        }
        // Should save the current position and rotation here
        lastPosition = position;
        lastRotation = rotation;
        gotLastPosition = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure to tag grabbable objects with the "grabbable" tag
        // You also need to make sure to have colliders for the grabbable objects and the controllers
        // Make sure to set the controller colliders as triggers or they will get misplaced
        // You also need to add Rigidbody to the controllers for these functions to be triggered
        // Make sure gravity is disabled though, or your controllers will (virtually) fall to the ground
        Transform t = other.transform;
        if(t && t.tag.ToLower()=="grabbable")
            nearObjects.Add(t);
        if(t && t.tag.ToLower()=="plug")
            plugs.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if( t && t.tag.ToLower()=="grabbable")
            nearObjects.Remove(t);
        if(t && t.tag.ToLower()=="plug")
            plugs.Remove(t);

    }
} 
