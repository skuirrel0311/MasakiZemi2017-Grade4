using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestObject : MonoBehaviour
{
    public Vector3 pos;
    public string particleName;

    void Start()
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        Debug.Log("rend = " + rends.Length);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            ParticleManager.I.Play(particleName, pos);
        }
    }
}
