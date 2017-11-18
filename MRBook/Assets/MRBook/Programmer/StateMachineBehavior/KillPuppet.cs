using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPuppet : BaseStateMachineBehaviour
{
    [SerializeField]
    ActorName puppetName = ActorName.Urashima;

    protected override void OnStart()
    {
        base.OnStart();

        HoloPuppet puppet = (HoloPuppet)ActorManager.I.GetCharacter(puppetName);

        puppet.puppet.state = RootMotion.Dynamics.PuppetMaster.State.Dead;
    }
}
