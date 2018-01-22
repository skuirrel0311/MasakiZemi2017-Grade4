using System.Collections;
using UnityEngine;

public class UrashimaGo : MoveCharacter
{
    HoloPuppet puppet;

    Coroutine coroutine;

    protected override void OnStart()
    {
        puppet = (HoloPuppet)ActorManager.I.GetObject(objectName);
        base.OnStart();
        
        coroutine = StateMachineManager.I.StartCoroutine(MonitorPuppet());
    }

    IEnumerator MonitorPuppet()
    {
        while (true)
        {
            if (puppet.Puppet.urashimaState == RootMotion.Dynamics.PuppetMaster.UrashimaState.Dead) break;
            yield return null;
        }

        CurrentStatus = BehaviourStatus.Failure;
        OnEnd();
    }

    protected override void OnEnd()
    {
        StateMachineManager.I.StopCoroutine(coroutine);
        base.OnEnd();
    }
}
