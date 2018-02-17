using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRemainParticle : BaseStateMachineBehaviour
{
    [SerializeField]
    string particleName = "";

    [SerializeField]
    string targetPointName = "";

    [SerializeField]
    float duration = 1.0f;

    protected override void OnStart()
    {
        base.OnStart();

        Transform targetPoint = null;
        if (!string.IsNullOrEmpty(targetPointName))
        {
            targetPoint = ActorManager.I.GetTargetPoint(targetPointName);
        }

        if (targetPoint == null)
            ParticleManager.I.Play(particleName, duration);
        else
            ParticleManager.I.Play(particleName, targetPoint.position, targetPoint.rotation, duration);
    }
}
