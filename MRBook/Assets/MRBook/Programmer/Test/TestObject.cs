using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestObject : MonoBehaviour
{
    NavMeshAgent agent;

    public GameObject obj;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            obj.transform.Translate(Vector3.right * 3);
        }
    }
}
