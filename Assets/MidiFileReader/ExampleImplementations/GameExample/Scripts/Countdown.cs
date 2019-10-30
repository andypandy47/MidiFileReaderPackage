using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiFileReader;
using UnityEngine.UI;
using DG.Tweening;

public class Countdown : MonoBehaviour
{
    private Text countdownText;
    private int countdownNumber;
    private Color startColor;

    private void Start()
    {
        countdownText = GetComponent<Text>();
        startColor = Color.white;
        countdownNumber = 4;
    }

	public void OnCountdown(NoteEventData data)
    {
        countdownText.material.color = startColor;
        countdownNumber--;

        if (countdownNumber >= 1)
        {
            ShowCountdownText(countdownNumber.ToString());
            countdownText.material.DOFade(0, 1.2f);
        }
        else
        {
            ShowCountdownText("Go!");
            countdownText.material.DOFade(0, 1.0f);
        }
          

    }

    private void ShowCountdownText(string textToShow)
    {
        countdownText.text = textToShow;
    }

    private void OnApplicationQuit()
    {
        countdownText.material.color = startColor;
    }
}
