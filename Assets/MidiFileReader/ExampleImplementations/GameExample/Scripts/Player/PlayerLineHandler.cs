using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLineHandler : MonoBehaviour {

    public static PlayerLineHandler instance;
    private LineRenderer lr;
    private Vector3 prevTailPos;
    private Coroutine fadeLineCoroutine;
    public float tailFadeSpeed;
    public float tailLength;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void UpdateLine(Vector3 fromPos)
    {
        if (fadeLineCoroutine != null)
            StopCoroutine(fadeLineCoroutine);

        Vector3 tailPos = (fromPos - PlayerController.instance.transform.position) * tailLength;
        lr.SetPosition(1, tailPos);
        prevTailPos = tailPos;
    }

    public void ResetLine()
    {
        lr.SetPosition(1, Vector3.zero);
    }

    public void FadeLine()
    {
        fadeLineCoroutine = StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float lerpVal = 0.0f;
        while(prevTailPos != Vector3.zero)
        {
            lerpVal += Time.deltaTime * tailFadeSpeed;
            prevTailPos = Vector3.Lerp(prevTailPos, Vector3.zero, lerpVal);
            lr.SetPosition(1, prevTailPos);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
