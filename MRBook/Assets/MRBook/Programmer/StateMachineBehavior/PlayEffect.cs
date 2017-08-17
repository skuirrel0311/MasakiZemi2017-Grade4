using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEffect : StateMachineBehaviour
{
    public string targetPositionName;
    public string effectName;
    public Vector3 offset;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 targetPosition = ActorManager.I.GetTargetPoint(targetPositionName).position;
        ParticleManager.I.Play(effectName, targetPosition + offset);
    }
}
