using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestObject : MonoBehaviour
{
    [SerializeField]
    BoxCollider box = null;

    void Start()
    {
        Debug.Log("(" + box.size.x + "," + box.size.y + "," + box.size.z + ")");
    }
}
