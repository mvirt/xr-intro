using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public string ButtonLetter;

    public bool pressed;

    public GameObject buttonVisual;
    public float pressDepth;
    private Vector3 originalPosition;
    private GameObject enigma;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = buttonVisual.transform.position;
        enigma = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Finger") {
            enigma.SendMessage("ButtonPressed", ButtonLetter);
            pressed = true;
            buttonVisual.transform.position = new Vector3(originalPosition.x, originalPosition.y - pressDepth, originalPosition.z);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Finger") {
            pressed = false;
            enigma.SendMessage("ButtonReleased", ButtonLetter);
            buttonVisual.transform.position = originalPosition;
        }
    }
}
