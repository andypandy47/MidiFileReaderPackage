using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using MidiFileReader;

[RequireComponent(typeof(UnityMainThreadDispatcher))]
public class MasterClock : MonoBehaviour
{
    public static MasterClock instance;
    [SerializeField] public bool playOnAwake = false;
    private bool countStarted = false;

    private List<MidiEventListener> listeners;

    [SerializeField]
    private float timePosMS;
    private float dspTime;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        timePosMS = 0.0f;

        if (playOnAwake)
            StartCounter();
    }

    public void Update()
    {
        
    }

    public void StartCounter()
    {
        countStarted = true;
        dspTime = (float)AudioSettings.dspTime;
    }

    public void StopCounter()
    {
        countStarted = false;
    }

    public void ResetCounter()
    {
        timePosMS = 0;
    }

    public void SubscribeEventListener(MidiEventListener listenerToAdd)
    {
        if (listeners == null)
            listeners = new List<MidiEventListener>();

        listeners.Add(listenerToAdd);
    }

    public float GetCurrentTimeInMS()
    {
        return timePosMS;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (countStarted)
        {
            timePosMS = (float)(AudioSettings.dspTime - dspTime) * 1000;

            if (listeners != null)
            {
                for (int i = 0; i < listeners.Count; i++)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(listeners[i].UpdateTimer(timePosMS));
                }
            }
        }
    }
}


