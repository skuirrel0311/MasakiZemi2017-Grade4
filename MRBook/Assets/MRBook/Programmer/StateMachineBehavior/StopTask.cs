using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTask : BaseStateMachineBehaviour
{
    public string taskName = "";

    public override void OnStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStart(animator, stateInfo, layerIndex);
        StateMachineManager.I.Stop(taskName);
    }
}