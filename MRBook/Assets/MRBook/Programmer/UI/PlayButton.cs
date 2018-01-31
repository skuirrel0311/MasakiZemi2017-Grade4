using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : HoloButton
{
    protected override void Start()
    {
        base.Start();
        MainSceneManager sceneManager = MainSceneManager.I;

        onClick.AddListener(()=>
        {
            sceneManager.Play();
        });

        sceneManager.OnPageInitialized += (page)=> Refresh();
        sceneManager.OnPlayPage += () => Disable();
        sceneManager.OnReset += () => Refresh();
    }
}
