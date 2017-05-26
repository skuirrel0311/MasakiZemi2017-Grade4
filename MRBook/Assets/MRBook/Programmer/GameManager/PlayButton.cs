using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class PlayButton : MonoBehaviour, IInputClickHandler
{
    bool isClick;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (isClick) return;
        isClick = true;
        MainGameManager.I.GameStart();
        gameObject.SetActive(false);
    }
}
