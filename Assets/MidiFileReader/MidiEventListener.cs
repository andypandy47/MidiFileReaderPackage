using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MidiFileReader
{

    [System.Serializable]
    public class NoteEvent : UnityEvent<NoteEventData> { }

    public class MidiEventListener : MonoBehaviour
    {
        private Timer timer; //this will go through the midi track and tell us when each event has happened
        [Tooltip("The file path of the midi file you wish to listen to")]
        public string midiFilePath;
        [Tooltip("This number tells the listener which track in the midi file to listen to for events. Type 0 midi files contain only one track so this can be" +
            "left as 0. Type 1 midi files contain multiple tracks, with the first track at index 0 containing meta events like tempo, time signature etc. If the midi file is type" +
            "one, and you want to listen for note events, then this number should at least be one")]
        public int trackNumberInFile = 1;

        [Space(5)]
        [EnumFlag]
        public NoteEventType eventTypesToListenFor;

        [Range(0, 120)]
        public int filterNotesAbove = 120;
        [Range(0, 120)]
        public int filterNotesBelow;

        [Tooltip("Selecting this will allow you to define what specific notes you want to listen for in the list NoteValuesToListenFor. If not selected" +
            " events will be triggered based on the filters specified above")]
        public bool listenForSpecificNotes = false;
        [Tooltip("Listen for specific notes needs to ticked for this to apply. Allows the listening of specific notes")]
        public List<int> noteValuesToListenFor;

        public NoteEvent noteOnEvent;
        public NoteEvent noteOffEvent;


        private void Start()
        {
            if (string.IsNullOrEmpty(midiFilePath))
            {
                Debug.LogError("No file path given for event listener on object: " + this.gameObject.name);
                return;
            }

            //create timer and subsribe to master clock
            timer = new Timer(this);


            if (MasterClock.instance != null)
                MasterClock.instance.SubscribeEventListener(this);
            else
            {
                Debug.LogError("Master clock component has not been created!...Add the master clock component to one of your gameobjects to update the event listeners");
                return;
            }
                
        }

        public IEnumerator<float> UpdateTimer(float timePosMS)
        {
            timer.UpdateTimer(timePosMS);
            yield return 0.0f;
        }

        public void RaiseNoteOnEvent(NoteEventData data)
        {
            noteOnEvent.Invoke(data);
        }

        public void RaiseNoteOffEvent(NoteEventData data)
        {
            noteOffEvent.Invoke(data);
        }
    }

}
