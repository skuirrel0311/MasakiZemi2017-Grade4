using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HoloToolkit.Unity.InputModule;

public class HoloButton : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    UnityEvent onClick;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        gameObject.SetActive(false);
        onClick.Invoke();
    }

    public void Refresh()
    {
#if UNITY_EDITOR

#else
        gameObject.SetActive(true);
#endif
    }
    public void Hide()
    {
#if UNITY_EDITOR

#else
        gameObject.SetActive(false);
#endif
    }
}
