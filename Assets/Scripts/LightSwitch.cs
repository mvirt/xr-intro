using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    private Light lightComponent;
    public InputActionReference action;
    // Start is called before the first frame update
    void Start()
    {
        lightComponent = gameObject.GetComponent<Light>();
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            Debug.Log("Light color changed");
            if(lightComponent.color == Color.white) {
                lightComponent.color = Color.blue;
            } else {
                lightComponent.color = Color.white;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
