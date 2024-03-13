using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    Animator animator;
    public float gripTarget;
    public float triggerTarget;

    private float gripCurrent;
    private float triggerCurrent;

    public float speed = 1.0f;

    public List<GameObject> fingers;
    List<Collider> fingerColliders;

    public List<GameObject> pinchFingers;
    List<Collider> pinchColliders;

    public Transform pinchPosition;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        fingerColliders = new List<Collider>();
        foreach(var finger in fingers) {
            fingerColliders.Add(finger.GetComponent<Collider>());
        }
        pinchColliders = new List<Collider>();
        foreach(var finger in pinchFingers) {
            pinchColliders.Add(finger.GetComponent<Collider>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();

        //Disable the finger colliders when in a fist
        SetFingerColliders(gripCurrent <= 0.5f);
    }

    void SetFingerColliders(bool state) {
        if (fingerColliders.Count > 0) {
            foreach(var collider in fingerColliders) {
                collider.enabled = state;
            }
        }
    }

    internal void SetGrip(float v) {
        gripTarget = v;
    }

    internal void SetTrigger(float v) {
        triggerTarget = v;
    }

    void AnimateHand()
    {
        if (gripCurrent != gripTarget) {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
            animator.SetFloat("Grip", gripCurrent);
        }
        if (triggerCurrent != triggerTarget) {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
            animator.SetFloat("Trigger", triggerCurrent);
        }
    }
}
