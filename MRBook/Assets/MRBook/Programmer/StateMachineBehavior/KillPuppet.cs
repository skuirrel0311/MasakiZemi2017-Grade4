using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class KillPuppet : BaseStateMachineBehaviour
{
    [SerializeField]
    ActorName puppetName = ActorName.Urashima;

    [SerializeField]
    float duration = 3.0f;

    protected override void OnStart()
    {
        base.OnStart();

        HoloPuppet puppet = (HoloPuppet)ActorManager.I.GetCharacter(puppetName);

        puppet.PuppetBehaviour.enabled = false;


        StateMachineManager.I.StartCoroutine(Utilities.FloatLerp(duration, (t) =>
        {
            puppet.Puppet.pinWeight = Mathf.Lerp(1.0f, 0.0f, t);
            puppet.Puppet.muscleWeight = Mathf.Lerp(1.0f, 0.0f, t);
        }
        ).OnCompleted(() =>
        {
            puppet.Puppet.urashimaState = RootMotion.Dynamics.PuppetMaster.UrashimaState.Dead;
            CurrentStatus = BehaviourStatus.Success;
            OnEnd();
        }));

    }

    protected override BehaviourStatus OnUpdate()
    {
        return BehaviourStatus.Running;
    }
}
