using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class HoloWindow : MonoBehaviour
{
    [SerializeField]
    TextMesh title = null;
    [SerializeField]
    TextMesh message = null;
    [SerializeField]
    HoloButton closeButton = null;

    Color clearColor = Color.clear;

    MyCoroutine viewCoroutine;

    Material[] imageMaterials;

    protected void Start()
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        imageMaterials = new Material[rends.Length];

        for(int i = 0;i< rends.Length;i++)
        {
            imageMaterials[i] = rends[i].material;
        }
    }

    public void Show(string title, string message)
    {

    }

    public void Close()
    {

    }
}
