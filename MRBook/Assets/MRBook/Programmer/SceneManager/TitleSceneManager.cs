using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    Transform bookPosition = null;

    [SerializeField]
    Transform bookPositionController = null;

    [SerializeField]
    TitleView titleView = null;

    public void GameStart()
    {
        bookPosition.position = bookPositionController.position;
        bookPosition.rotation = bookPositionController.rotation;
        titleView.HideAll();
        
        SceneManager.LoadSceneAsync("main");
    }
}
