using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestObject : MonoBehaviour
{
    public Vector3 pos;
    public string particleName;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            ParticleManager.I.Play(particleName, pos);
        }
    }
}
