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

        sceneManager.OnPageInitialized += (page) =>
        {
            if (page.Index == 4)
                Disable();
            else
                Refresh();
        };
        sceneManager.OnReset += () =>
        {
            Utilities.Delay(25, () => Refresh(), this);
        };
        sceneManager.OnPlayEnd += (success) =>
        {
            if (!success) Refresh();
        };
    }
}
