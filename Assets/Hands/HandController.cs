using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    public Hand hand;
    public InputActionReference gripAction;
    public InputActionReference pinchAction;
    // Start is called before the first frame update
    void Start()
    {
        gripAction.action.Enable();
        pinchAction.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        float grip = 0.0f;
        if (gripAction.action.IsPressed()) {
            grip = 1.0f;
        }
        float pinch = 0.0f;
        if (pinchAction.action.IsPressed()) {
            pinch = 1.0f;
        }
        hand.SetGrip(grip);
        hand.SetTrigger(pinch);
    }
}
