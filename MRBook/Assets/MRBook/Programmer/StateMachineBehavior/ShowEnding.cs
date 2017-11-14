using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowEnding : BaseStateMachineBehaviour
{
    public string endingMessage = "死因：顔面強打";

    public override void OnStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStart(animator, stateInfo, layerIndex);
        MainGameUIController.I.endingManager.Show(endingMessage);
    }
}
