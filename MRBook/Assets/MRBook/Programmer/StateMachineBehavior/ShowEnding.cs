using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowEnding : StateMachineBehaviour
{
    public string endingMessage = "死因：顔面強打";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MainGameUIController.I.endingManager.Show(endingMessage);
    }
}
