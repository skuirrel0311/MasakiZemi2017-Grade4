using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePuppet :BaseStateMachineBehaviour
{
    ActorName puppetName = ActorName.Urashima;

    protected override void OnStart()
    {
        base.OnStart();

        HoloPuppet puppet = (HoloPuppet)ActorManager.I.GetCharacter(puppetName);

        puppet.Puppet.mode = RootMotion.Dynamics.PuppetMaster.Mode.Kinematic;
    }
}
