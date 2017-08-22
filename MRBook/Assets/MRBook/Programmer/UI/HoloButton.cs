using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HoloToolkit.Unity.InputModule;

public class HoloButton : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    UnityEvent onClick = null;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        gameObject.SetActive(false);
        onClick.Invoke();
    }

    public void Refresh()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
