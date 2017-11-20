using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class DelayTask : BaseStateMachineBehaviour
{
    [SerializeField]
    float duration = 1.0f;

    protected override void OnStart()
    {
        base.OnStart();
        StateMachineManager.I.StartCoroutine(Utilities.Delay(duration, () =>
        {
            CurrentStatus = BehaviourStatus.Success;
            OnEnd();
        }));
    }

    protected override BehaviourStatus OnUpdate()
    {
        return BehaviourStatus.Running;
    }
}
