using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTask : StateMachineBehaviour
{
    public string taskName = "";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StateMachineManager.I.Stop(taskName);
    }
}