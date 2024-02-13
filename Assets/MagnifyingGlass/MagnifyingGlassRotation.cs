using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnifyingGlassRotation : MonoBehaviour
{
    public GameObject vrCamera;
    public Transform vrCameraTempTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vrCameraTempTransform.LookAt(transform, transform.parent.up);
        //transform.LookAt(vrCamera.transform, transform.up);
        transform.rotation = vrCameraTempTransform.rotation;
    }
}
