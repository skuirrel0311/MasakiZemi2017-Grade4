using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    public void StartGame()
    {
        LoadSceneManager.I.LoadScene("Matching", true, 1.0f);
    }
}
