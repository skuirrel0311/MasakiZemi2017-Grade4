using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPuppet : BaseStateMachineBehaviour
{
    [SerializeField]
    ActorName puppetName = ActorName.Urashima;

    public override void OnStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStart(animator, stateInfo, layerIndex);

        HoloPuppet puppet = (HoloPuppet)ActorManager.I.GetCharacter(puppetName);

        puppet.puppet.state = RootMotion.Dynamics.PuppetMaster.State.Dead;
    }
}
