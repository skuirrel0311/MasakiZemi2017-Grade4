using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTask : BaseStateMachineBehaviour
{
    public string taskName = "";

    protected override void OnStart()
    {
        base.OnStart();
        StateMachineManager.I.Stop(taskName);
    }
}