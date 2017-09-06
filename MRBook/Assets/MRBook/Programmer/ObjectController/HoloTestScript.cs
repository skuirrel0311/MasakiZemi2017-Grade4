using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;
using HoloToolkit.Unity.InputModule;

public class HoloTestScript : MonoBehaviour, IInputClickHandler
{
    MyWorldAnchorManager anchorSroreManager;

    GameObject[] worldAnchors;
    Renderer m_rendere;

    //うごかせるか？
    bool isMovable = false;

    void Start()
    {
        anchorSroreManager = MyWorldAnchorManager.I;
        m_rendere = GetComponent<Renderer>();

        worldAnchors = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            worldAnchors[i] = transform.GetChild(i).gameObject;
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        isMovable = !isMovable;
        Debug.Log("taped");

        if (isMovable)
        {
            for (int i = 0; i < worldAnchors.Length; i++)
            {
                DestroyImmediate(worldAnchors[i].GetComponent<WorldAnchor>());
                anchorSroreManager.anchorStore.Delete(worldAnchors[i].name);
            }
            GetComponent<MyObjPositionController>().enabled = true;
            m_rendere.material.color = Color.green;
        }
        else
        {
            for (int i = 0; i < worldAnchors.Length; i++)
            {
                anchorSroreManager.SaveAnchor(worldAnchors[i]);
            }
            GetComponent<MyObjPositionController>().enabled = false;
            m_rendere.material.color = Color.red;
        }
    }
}
