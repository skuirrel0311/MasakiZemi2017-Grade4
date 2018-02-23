using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHint : BaseStateMachineBehaviour
{
    [SerializeField]
    int hintCode = 0;

    protected override void OnStart()
    {
        base.OnStart();
        MainSceneManager sceneManager = MainSceneManager.I;

        MissionTextController.I.SetHint(sceneManager.pages[sceneManager.currentPageIndex].hintTexts[hintCode]);
    }
}
