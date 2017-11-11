using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLog : BaseStateMachineBehaviour
{
    [SerializeField]
    public string text = "";

    public override void OnStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStart(animator, stateInfo, layerIndex);
        Debug.Log(text);
    }

    public override BehaviourStatus OnUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        return BehaviourStatus.Success;
    }
}
