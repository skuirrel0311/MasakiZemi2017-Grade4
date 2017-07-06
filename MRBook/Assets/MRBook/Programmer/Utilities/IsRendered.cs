using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsRendered : MonoBehaviour
{
    bool isRendered = false;
    public bool WasRendered { get; private set; }
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        WasRendered = isRendered;
        isRendered = false;
    }

    void OnWillRenderObject()
    {
        if (Camera.current.tag == mainCamera.tag)
        {
            isRendered = true;
        }
    }
}
