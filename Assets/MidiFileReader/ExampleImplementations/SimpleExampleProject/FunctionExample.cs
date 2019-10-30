using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiFileReader;

public class FunctionExample : MonoBehaviour
{
    public void OnNoteOnEvent(NoteEventData data)
    {
        Debug.Log("Note on: " + data.noteValue);
    }
}
