using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTestSceneManager : MonoBehaviour
{
    public string particleName = "";

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            ParticleManager.I.Play(particleName, Vector3.zero);
        }
    }
}
