using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestObject : MonoBehaviour
{
    void Start()
    {
        float x = 3.541592f;
        float size = 8f;
        Debug.Log("x = " + size * Mathf.Floor(x / size));
        Debug.Log("x = " + Mathf.Floor(x));
    }
}
