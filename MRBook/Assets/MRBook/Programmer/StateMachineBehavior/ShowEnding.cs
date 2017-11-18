using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowEnding : BaseStateMachineBehaviour
{
    public string endingMessage = "死因：顔面強打";

    protected override void OnStart()
    {
        base.OnStart();
        MainGameUIController.I.endingManager.Show(endingMessage);
    }
}
