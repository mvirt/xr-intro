using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchableObject : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public GameObject grabbable;
    public GameObject controlledObject;

    public float stepSize = 0.1f;
    private float lastZ;
    public int allowedStepsOneGo = 3;
    private int takenSteps;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = grabbable.transform.localPosition;
        originalRotation = grabbable.transform.localRotation;
        lastZ = originalPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        var posZ = grabbable.transform.localPosition.z;
        var delta = posZ - lastZ;
        var step = Mathf.RoundToInt(delta / stepSize);
        if (step >= 1 && (takenSteps < allowedStepsOneGo)) {
            controlledObject.SendMessage("TickForward");
            takenSteps += 1;
            lastZ = posZ;
        } else if(step <= -1 && (takenSteps > -allowedStepsOneGo)) {
            controlledObject.SendMessage("TickBackward");
            takenSteps -= 1;
            lastZ = posZ;
        }
    }

    void Reset() {
        //Reset the position of the grabbable object
        grabbable.transform.localPosition = originalPosition;
        grabbable.transform.localRotation = originalRotation;
        takenSteps = 0;
        lastZ = originalPosition.z;
    }
}
