using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenShake : MonoBehaviour {

    public static void Shake(float duration)
    {
        Camera.main.DOShakePosition(duration, 0.5f, 25, 90, true);
    }
}
