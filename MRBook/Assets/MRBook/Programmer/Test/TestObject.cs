using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    [SerializeField]
    IsRendered front = null;

    Renderer m_renderer;

    void Start()
    {
        m_renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if(front.WasRendered)
        {
            m_renderer.material.color = Color.red;
        }
        else
        {
            m_renderer.material.color = Color.white;
        }
    }
}
