using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiFileReader;

public class GrowingThingSpawner : MonoBehaviour
{
    public GameObject gowingThingPrefab;
    private GrowingThing currentGrowingThing;

    public void OnNoteOnEvent(NoteEventData data)
    {
        SpawnGrowingThing();
    }

    public void OnNoteOffEvent(NoteEventData data)
    {
        ExplodeThing();
    }

    private void SpawnGrowingThing()
    {
        GameObject go = Instantiate(gowingThingPrefab, this.transform);
        go.transform.position = transform.position;
        currentGrowingThing = go.GetComponent<GrowingThing>();
    }

    private void ExplodeThing()
    {
        StartCoroutine(currentGrowingThing.Explode());
    }



	
}
