using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiFileReader;

public class LightFlicker : MonoBehaviour {

    private Light pointLight;
    private bool lightEnabled = true;

    private void Start()
    {
        pointLight = GetComponent<Light>();
    }

    public void FlickerLight(NoteEventData data)
    {
        lightEnabled = !lightEnabled;
        pointLight.enabled = lightEnabled;
    }
}
