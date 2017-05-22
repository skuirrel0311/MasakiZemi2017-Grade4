using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class TapToStartMessage : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    GameObject anchor;

    Vector3 offset = new Vector3(-0.62f, 0.53f, 0.0f);

    GameObject mainCamera;

    public bool IsGameStart = false;

    void Start()
    {
        mainCamera = Camera.main.gameObject;
    }

    void Update()
    {
        if (IsGameStart) return;
        transform.position = anchor.transform.position + offset;
        transform.LookAt(mainCamera.transform);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (IsGameStart) return;
        anchor.gameObject.SetActive(false);
        GetComponent<TextMesh>().text = "Game Start !!";
        IsGameStart = true;
        gameObject.SetActive(false);
    }
}
