using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    private Light light;
    public InputActionReference action;
    // Start is called before the first frame update
    void Start()
    {
        light = gameObject.GetComponent<Light>();
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            Debug.Log("Light color changed");
            if(light.color == Color.white) {
                light.color = Color.blue;
            } else {
                light.color = Color.white;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
