using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestObject : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("KanekoTest");
        }
    }
}
