using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enigma : MonoBehaviour
{
    public List<GameObject> displayLetters;

    public Material displayLit;
    public Material displayUnLit;

    public List<GameObject> rolls;

    public float buttonTimeOut = 0.7f;
    private float lastButtonPress = -1;

    public List<string> rollWirings;
    public List<string> rollReverseWirings;
    public string rollNotches;

    public List<string> patchConnections;

    public TextMeshPro textDisplay;
    public int maxCharCount = 15;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        textDisplay.text = "";
        audioSource = GetComponent<AudioSource>();
    }

    char PlugBoardTranslate(char input) {
        foreach(var patch in patchConnections) {
            if(patch.Contains("" + input)) {
                var a = patch[0];
                var b = patch[1];
                if (input == a) {
                    return b;
                }
                if (input == b) {
                    return a;
                }
            }
        }
        return input;
    }

    char Cipher(char input) {
        var letter = PlugBoardTranslate(input);
        int index;

        //Debug.Log("Roll 1 Pos: " + rolls[0].GetComponent<EnigmaRoll>().GetPosition());
        //Debug.Log("Roll 2 Pos: " + rolls[1].GetComponent<EnigmaRoll>().GetPosition());
        var roll1Notch = rolls[0].GetComponent<EnigmaRoll>().GetPosition() == rollNotches[0];
        var roll2Notch = rolls[1].GetComponent<EnigmaRoll>().GetPosition() == rollNotches[1];

        if (roll1Notch && roll2Notch) {
            TurnRoll(3);
        }
        if (roll1Notch) {
            TurnRoll(2);
        }
        TurnRoll(1);

        //Rolls cipher "right"
        for (int i = 1; i < 4; i++) {
            var rollPosition = rolls[i - 1].GetComponent<EnigmaRoll>().position - 1;
            //Debug.Log("Roll " + i + " position: " + rollPosition);

            index = (letter - 65) % 26;
            //Debug.Log("Letter: " + letter + " index: " + index);
            index = (index + rollPosition) % 26;

            letter = rollWirings[i][index];
            letter = (char)(65 + (letter - 65 + 26 - rollPosition) % 26);
            //Debug.Log("Ciphered: " + letter);
        }

        int reflectorPosition = 0;
        //Reflector cipher
        index = (letter - 65) % 26;
        index = (index + reflectorPosition) % 26;

        letter = rollWirings[0][index];
        letter = (char)(65 + (letter - 65 + 26 - reflectorPosition) % 26);

        //Rolls cipher "left"
        for (int i = 3; i > 0; i--) {
            var rollPosition = rolls[i - 1].GetComponent<EnigmaRoll>().position - 1;
            index = (letter - 65) % 26;
            index = (index + rollPosition) % 26;
            letter = rollReverseWirings[i][index];
            letter = (char)(65 + (letter - 65 + 26 - rollPosition) % 26);
        }
        return PlugBoardTranslate(letter);
    }

    public void Connected(string pair) {
        patchConnections.Add(pair);
    }

    public void Disconnected(string pair) {
        patchConnections.Remove(pair);
    }

    GameObject FindFromList(string letter) {
        foreach (var obj in displayLetters) {
            if(obj.name.EndsWith(letter)) {
                return obj;
            }
        }
        return null;
    }

    public void TurnRoll(int index) {
        rolls[index - 1].SendMessage("TickForward");
    }

    public void ButtonPressed(string letter) {
        //Debug.Log(letter + " pressed");
        if ((Time.time - lastButtonPress) > buttonTimeOut) {
            audioSource.Play(0);
            //Debug.Log("Activated " + letter);
            var cipheredChar = Cipher(letter[0]);
            SetDisplayLetter("" + cipheredChar, true);
            textDisplay.text += "" + cipheredChar;
            var charCount = textDisplay.text.Length;
            if (charCount > maxCharCount) {
                textDisplay.text = textDisplay.text.Substring(charCount - maxCharCount);
            }
            lastButtonPress = Time.time;
        }
    }

    public void ButtonReleased(string letter) {
        //Debug.Log(letter + " released");
        //SetDisplayLetter(letter, false);
        ClearDisplay();
    }

    void ClearDisplay() {
        foreach (var obj in displayLetters) {
            obj.GetComponent<MeshRenderer>().material = displayUnLit;
        }
    }

    void SetDisplayLetter(string letter, bool status) {
        var obj = FindFromList(letter);
        if (obj == null) {
            return;
        }
        if (status) {
            obj.GetComponent<MeshRenderer>().material = displayLit;
        } else {
            obj.GetComponent<MeshRenderer>().material = displayUnLit;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
