using System.Collections;
using UnityEngine;

public class UrashimaGo : MoveCharacter
{
    HoloPuppet puppet;

    [SerializeField]
    float rotationSpeed = 200.0f;
    Quaternion to;

    bool isRotating = false;

    [SerializeField]
    bool callChangeAnimation = true;

    protected override void OnStart()
    {
        puppet = (HoloPuppet)ActorManager.I.GetObject(objectName);
        base.OnStart();
        
        StateMachineManager.I.StartCoroutine(MonitorPuppet());
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
        StateMachineManager.I.StopCoroutine(MonitorPuppet());
        base.OnEnd();
    }
}
