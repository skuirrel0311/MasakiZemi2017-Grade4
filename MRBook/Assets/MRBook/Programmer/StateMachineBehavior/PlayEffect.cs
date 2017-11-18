using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEffect : BaseStateMachineBehaviour
{
    public string targetPositionName;
    public string effectName;
    public Vector3 offset;

    protected override void OnStart()
    {
        base.OnStart();
        Vector3 targetPosition = ActorManager.I.GetTargetPoint(targetPositionName).position;
        ParticleManager.I.Play(effectName, targetPosition + offset);
    }
}
