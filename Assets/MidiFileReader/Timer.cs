using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

namespace MidiFileReader
{

    public class Timer
    {
        public bool instrumentPlaying = false;

        private TrackInfo track;

        private NoteEventData lastNoteEvent;

        private float counterTimeMS;
        private float dspTime;

        private float lastEventTimeMS = 0;

        private NoteEventType eventTypesToListenFor;
        private delegate void RaiseEvent(NoteEventData data);
        private RaiseEvent raiseNoteOnEvent;
        private RaiseEvent raiseNoteOffEvent;
        private int filterNotesAbove;
        private int filterNotesBelow;
        private bool listenForSpecificNotes = false;
        private List<int> notesToListenFor;

        public Timer(MidiEventListener eventListener)
        {
            track = MidiParse.ParseMidi(eventListener.midiFilePath, 0, eventListener.trackNumberInFile);
            eventTypesToListenFor = eventListener.eventTypesToListenFor;
            raiseNoteOnEvent = eventListener.RaiseNoteOnEvent;
            raiseNoteOffEvent = eventListener.RaiseNoteOffEvent;

            filterNotesAbove = eventListener.filterNotesAbove;
            filterNotesBelow = eventListener.filterNotesBelow;

            listenForSpecificNotes = eventListener.listenForSpecificNotes;

            if (listenForSpecificNotes && eventListener.noteValuesToListenFor != null)
                notesToListenFor = eventListener.noteValuesToListenFor;
            else if (listenForSpecificNotes && eventListener.noteValuesToListenFor == null)
                Debug.LogError("No notes have been set to listen for!...");

            lastEventTimeMS = 0.0f;
        }

        /// <summary>
        /// Updates timer position, detects and raises upcoming midi events.
        /// </summary>
        /// <param name="timePosMS"></param>
        public void UpdateTimer(float timePosMS)
        {
            //update timer position based on master clock
            counterTimeMS = timePosMS - lastEventTimeMS;

            //if event index is in range
            if (track.currentEventIndex < track.events.Count)
            {
                NoteEventData nextEvent = track.events[track.currentEventIndex];
                //check counter time is greater than or equal to the time of the next event
                if (counterTimeMS >= nextEvent.timeMS)
                {
                    //Go through a series of checks to make sure we have put the right variables in to access the event
                    if (NoteEventIsAllowed(nextEvent))
                    {
                        switch (nextEvent.type)
                        {
                            case NoteEventType.NoteOn:
                                raiseNoteOnEvent(nextEvent);
                                break;
                            case NoteEventType.NoteOff:
                                raiseNoteOffEvent(nextEvent);
                                break;
                        }
                    }

                    //if reaching this point then an event has happened, whether or not we want it too,
                    //therefore to get the next one these variables must be modified so that the counter can continue on to the next event
                    //or else we will be stuck on the same event each update cycle
                    lastEventTimeMS += nextEvent.timeMS;
                    track.currentEventIndex++;
                }
            }
            else
            {
                instrumentPlaying = false;
                MasterClock.instance.StopCounter();
            }

        }

        private void ResetCounterTime(float timePosMS)
        {
            lastEventTimeMS = timePosMS;
        }

        private bool NoteEventIsAllowed(NoteEventData noteEvent)
        {
            if (noteEvent.type == eventTypesToListenFor || eventTypesToListenFor == (NoteEventType)3)
            {
                if (noteEvent.noteValue > filterNotesBelow && noteEvent.noteValue < filterNotesAbove)
                {
                    if (listenForSpecificNotes)
                    {
                        if (notesToListenFor != null && notesToListenFor.Contains(noteEvent.noteValue))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }
    }


}
