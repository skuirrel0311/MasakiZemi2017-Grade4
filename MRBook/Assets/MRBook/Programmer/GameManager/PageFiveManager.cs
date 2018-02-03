using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class PageFiveManager : BasePage
{
    public override void PageStart()
    {
        base.PageStart();

        MainSceneManager sceneManager = MainSceneManager.I;

        sceneManager.OnPlayEnd += (success) =>
        {
            ResultManager.I.ShowTotalResult();

            Utilities.Delay(2.0f, () =>
            {
                ResultManager.I.ShowTitleBack();
            }, this);

        };
    }
}
