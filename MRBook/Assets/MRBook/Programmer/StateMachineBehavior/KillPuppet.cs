﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class KillPuppet : BaseStateMachineBehaviour
{
    [SerializeField]
    ActorName puppetName = ActorName.Urashima;

    protected override void OnStart()
    {
        base.OnStart();

        HoloPuppet puppet = (HoloPuppet)ActorManager.I.GetCharacter(puppetName);

        puppet.PuppetBehaviour.enabled = false;


        StateMachineManager.I.StartCoroutine(Utilities.FloatLerp(3.0f, (t) =>
        {
            puppet.Puppet.pinWeight = Mathf.Lerp(1.0f, 0.0f, t);
        }
        ).OnCompleted(() =>
        {
            puppet.Puppet.state = RootMotion.Dynamics.PuppetMaster.State.Dead;
            CurrentStatus = BehaviourStatus.Success;
            OnEnd();
        }));

    }

    protected override BehaviourStatus OnUpdate()
    {
        return BehaviourStatus.Running;
    }
}
