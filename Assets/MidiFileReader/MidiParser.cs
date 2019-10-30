using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using MidiSharp;
using MidiSharp.Events;
using MidiSharp.Events.Voice.Note;
using MidiSharp.Events.Meta;

namespace MidiFileReader
{
    public enum NoteEventType
    {
        NoteOn = 1,
        NoteOff = 2,
    }
    public enum TimeSig { FourFour }

    public struct NoteEventData
    {
        public NoteEventData(float time, NoteEventType type, int noteValue, float velocity)
        {
            this.timeMS = time;
            this.type = type;
            this.noteValue = noteValue;
            this.velocity = velocity;
        }
        public readonly float timeMS;
        public readonly NoteEventType type;
        public readonly int noteValue;
        public readonly float velocity;
    }

    public class TrackInfo
    {
        public TrackInfo(List<NoteEventData> events, float tempo, TimeSig timeSig, int instrumentIndex)
        {
            this.events = events;
            this.tempo = tempo;
            this.timeSig = timeSig;
            this.instrumentIndex = instrumentIndex;
            currentEventIndex = 0;
        }
        public readonly List<NoteEventData> events;
        public float tempo;
        public TimeSig timeSig;
        public int currentEventIndex;
        public int instrumentIndex;

        public void PrintInfo()
        {
            Debug.Log("Tempo: " + tempo);
            Debug.Log("Time sig: " + timeSig.ToString());
            foreach (NoteEventData n in events)
            {
                Debug.Log(n.type == NoteEventType.NoteOn ? "Note on time: " + n.timeMS : "Note off time: " + n.timeMS);
            }
        }
    }

    public class MidiParse
    {
        public static TrackInfo ParseMidi(string filePath, int instrumentIndex = 0, int trackNumberToParse = 0)
        {
            MidiSequence sequence;

            using (Stream inputStream = File.OpenRead(filePath))
            {
                sequence = MidiSequence.Open(inputStream);
            }

            List<NoteEventData> eventInfo = new List<NoteEventData>();
            float tempo = 0;
            TimeSig timeSig = TimeSig.FourFour;
            float millisecondsPerTick = 0;

            MidiTrack metaEventTrack = sequence.Tracks[0];

            foreach (MidiEvent e in metaEventTrack)
            {
                Type t = e.GetType();
                if (t.Equals(typeof(TempoMetaMidiEvent)))
                {
                    TempoMetaMidiEvent tempoEvent = (TempoMetaMidiEvent)e;
                    float microsecondsPerBeat = tempoEvent.Value;
                    float millisecondsPerBeat = microsecondsPerBeat / 1000;
                    float secondsPerBeat = millisecondsPerBeat / 1000;
                    tempo = 60 / secondsPerBeat;

                    float microsecondsPerTick = microsecondsPerBeat / (float)sequence.TicksPerBeatOrFrame;
                    millisecondsPerTick = microsecondsPerTick / 1000;
                }
            }

            if (trackNumberToParse > sequence.Tracks.Count)
            {
                Debug.LogError("Track number specified is greater than amount of tracks in midi file!...");
                return null;
            }

            MidiTrack trackToParse = sequence.Tracks[trackNumberToParse];

            long absoluteTime = 0;
            foreach (MidiEvent e in trackToParse.Events)
            {
                absoluteTime += e.DeltaTime;
            }

            for (int i = 0; i < trackToParse.Events.Count; i++)
            {
                Type t = trackToParse.Events[i].GetType();

                if (t.Equals(typeof(OnNoteVoiceMidiEvent)))
                {
                    OnNoteVoiceMidiEvent onEvent = (OnNoteVoiceMidiEvent)trackToParse.Events[i];

                    float time = onEvent.DeltaTime * millisecondsPerTick;

                    NoteEventData Event = new NoteEventData(time, NoteEventType.NoteOn, MidiEvent.GetNoteValue(MidiEvent.GetNoteName(onEvent.Note)), onEvent.Velocity);
                    eventInfo.Add(Event);

                }
                else if (t.Equals(typeof(OffNoteVoiceMidiEvent)))
                {
                    OffNoteVoiceMidiEvent offEvent = (OffNoteVoiceMidiEvent)trackToParse.Events[i];

                    float time = offEvent.DeltaTime * millisecondsPerTick;

                    NoteEventData Event = new NoteEventData(time, NoteEventType.NoteOff, MidiEvent.GetNoteValue(MidiEvent.GetNoteName(offEvent.Note)), offEvent.Velocity);
                    eventInfo.Add(Event);
                }
                else if (t.Equals(typeof(TimeSignatureMetaMidiEvent)))
                {
                    TimeSignatureMetaMidiEvent timeSignature = (TimeSignatureMetaMidiEvent)trackToParse.Events[i];

                    if (timeSignature.Numerator == 4 && timeSignature.Denominator == 4)
                        timeSig = TimeSig.FourFour;
                }
            }
            return new TrackInfo(eventInfo, tempo, timeSig, instrumentIndex);
        }
    }
}


