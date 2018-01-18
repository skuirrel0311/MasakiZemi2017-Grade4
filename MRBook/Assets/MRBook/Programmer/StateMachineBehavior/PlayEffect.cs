using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEffect : BaseStateMachineBehaviour
{
    [SerializeField]
    ActorManager.TargetType targetType = ActorManager.TargetType.StaticPoint;
    [SerializeField]
    string targetName = "";
    [SerializeField]
    string effectName = "";
    [SerializeField]
    string WwiseEventName = string.Empty;
    [SerializeField]
    public Vector3 offset = Vector3.zero;

    protected override void OnStart()
    {
        base.OnStart();
        Transform target = ActorManager.I.GetTargetTransform(targetName, targetType);
        if(target == null)
        {
            Debug.LogError(target + "is not found");
            return;
        }

        ParticleManager.I.Play(effectName, target.position + offset);

        if(!string.IsNullOrEmpty(WwiseEventName))
        {
            AkSoundEngine.PostEvent(WwiseEventName, target.gameObject);
        }
    }
}
