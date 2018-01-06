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

    Dictionary<Renderer, Color> rendererDictionary = new Dictionary<Renderer, Color>();

    protected void Start()
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        for(int i = 0;i< rends.Length;i++)
        {
            //imageMaterials[i] = rends[i].material;
        }
    }

    public void Show(string title, string message)
    {

    }

    public void Close()
    {

    }
}
