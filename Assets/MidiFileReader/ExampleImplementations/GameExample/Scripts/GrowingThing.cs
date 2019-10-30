using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiFileReader;
using DG.Tweening;

public class GrowingThing : MonoBehaviour
{
    [SerializeField] private float maxSize = 15;
    [SerializeField] private readonly float size;
    [SerializeField] private bool growing;

    private GameObject sprite1;
    private ParticleSystem ps;
    
    private void OnEnable()
    {
        sprite1 = transform.GetChild(1).gameObject;
        ps = GetComponentInChildren<ParticleSystem>();
        Grow();
    }

    public void StopGrowing(NoteEventData data)
    {
        Explode();
    }

    private void Grow()
    {
        transform.DOScale(new Vector3(maxSize,maxSize,1), 1.8f);
    }

    public IEnumerator Explode()
    {
        transform.DOKill();
        sprite1.SetActive(false);
        ps.Play();
        yield return new WaitForSeconds(ps.main.duration);
        Destroy(this.gameObject);
    }
}
