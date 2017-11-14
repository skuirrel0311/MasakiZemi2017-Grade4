using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEffect : BaseStateMachineBehaviour
{
    public string targetPositionName;
    public string effectName;
    public Vector3 offset;

    public override void OnStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStart(animator, stateInfo, layerIndex);
        Vector3 targetPosition = ActorManager.I.GetTargetPoint(targetPositionName).position;
        ParticleManager.I.Play(effectName, targetPosition + offset);
    }
}
