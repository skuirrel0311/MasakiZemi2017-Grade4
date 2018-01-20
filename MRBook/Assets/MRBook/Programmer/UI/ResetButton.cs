using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

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
        sceneManager.OnReset += () =>
        {
            Utilities.Delay(4, () => Refresh(),this);
        };
        sceneManager.OnPlayPage += () => Disable();
        sceneManager.OnPlayEnd += (success) =>
        {
            if (!success) Refresh();
        };
    }
}
