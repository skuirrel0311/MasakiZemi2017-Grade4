using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : BaseManager<TitleSceneManager>
{
    public void GameStart()
    {
        SceneManager.LoadSceneAsync("main");
    }
}
