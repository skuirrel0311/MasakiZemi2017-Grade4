using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class PlayButton : MonoBehaviour, IInputClickHandler
{
    bool isPlaying;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (isPlaying) return;
        isPlaying = true;
        //MainGameManager.I.Play();
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        isPlaying = false;
        gameObject.SetActive(true);
    }
}
