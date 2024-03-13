using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmaRoll : MonoBehaviour
{
    public float stepSize = 13.3f;
    public int position = 1;
    public float rotationSpeed = 1;
    private float positionTarger;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public char GetPosition() {
        return (char)((position - 1) % 26 + 65);
    }

    void UpdateRotation() {
        var currentRotation = transform.eulerAngles.z;
        var rotationTarget = 90.0f + (position - 1) * stepSize;
        if (rotationTarget - currentRotation > 180) {
            currentRotation += 360.0f;
        }
        else if (rotationTarget - currentRotation > 180) {
            currentRotation -= 360.0f;
        }
        var rotation = Mathf.MoveTowards(currentRotation, rotationTarget, Time.deltaTime * rotationSpeed);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotation);
    }

    void TickForward() {
        audioSource.Play(0);
        position += 1;
        if (position > 26) {
            position = 1;
        }
    }

    void TickBackward() {
        audioSource.Play(0);
        position -= 1;
        if (position < 1) {
            position = 26;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
    }
}
