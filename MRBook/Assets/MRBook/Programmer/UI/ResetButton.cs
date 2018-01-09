using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : HoloButton
{
    protected override void Start()
    {
        base.Start();
        MainSceneManager sceneManager = MainSceneManager.I;

        onClick.AddListener(() =>
        {
            sceneManager.ResetPage();
        });

        sceneManager.OnPageLoaded += (page) => Refresh();
        sceneManager.OnPlayPage += () => Disable();
        sceneManager.OnPlayEnd += (success) => Refresh();
    }
}
