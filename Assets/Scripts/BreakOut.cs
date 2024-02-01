using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BreakOut : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool position = true;
    public Transform alternatePosition;
    public InputActionReference action;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        action.action.Enable();
        action.action.performed += (ctx) => {TogglePosition();};
    }

    void TogglePosition() {
        Debug.Log("Light color changed");
        if(position) {
            position = false;
            transform.position = alternatePosition.position;
        } else {
            position = true;
            transform.position = originalPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
