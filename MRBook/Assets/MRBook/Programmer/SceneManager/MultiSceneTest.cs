using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiSceneTest : MonoBehaviour
{
    int currentPageIndex = 0;
    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(currentPageIndex >= 1) SceneManager.UnloadSceneAsync("Page" + currentPageIndex.ToString());
            currentPageIndex++;
            SceneManager.LoadSceneAsync("Page" + currentPageIndex.ToString(), LoadSceneMode.Additive);
        }
    }
}
