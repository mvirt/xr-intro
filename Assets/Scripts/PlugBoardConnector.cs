using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugBoardConnector : MonoBehaviour
{
    public GameObject otherPlug;
    private LineRenderer line;
    public float cableWidth = 0.5f;
    public bool lineDrawer;
    private GameObject enigma;

    public string lastConnection = "";

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        enigma = GameObject.Find("enigma");
        if (lineDrawer) {
            line = GetComponent<LineRenderer>();
            line.widthMultiplier  = cableWidth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lineDrawer) {
            var newPositions = new Vector3[2];
            newPositions[0] = transform.position;
            newPositions[1] = otherPlug.transform.position;
            line.SetPositions(newPositions);
        }
    }

    public void Disconnected() {
        if (lastConnection.Length > 0) {
            audioSource.Play(0);
            enigma.SendMessage("Disconnected", lastConnection);
            lastConnection = "";
        }
    }

    public void Connected() {
        //The plug was released
        audioSource.Play(0);
        char letter1 = '0';
        char letter2 = '0';
        if (transform.parent) {
            letter1 = transform.parent.name[transform.parent.name.Length - 1];
        }
        if (otherPlug.transform.parent) {
            letter2 = otherPlug.transform.parent.name[otherPlug.transform.parent.name.Length - 1];
        }
        Debug.Log("Connected to " + letter1 + " and " + letter2);
        if (letter1 != '0' && letter2 != '0') {
            lastConnection = "" + letter1 + letter2;
            otherPlug.GetComponent<PlugBoardConnector>().lastConnection = lastConnection;
            enigma.SendMessage("Connected", lastConnection);
        }
    }
}
