using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpriteAnimation : BaseStateMachineBehaviour
{
    [SerializeField]
    ActorManager.TargetType targetType = ActorManager.TargetType.StaticPoint;
    [SerializeField]
    string targetName = "";
    [SerializeField]
    string spriteAnimationName = "";
    [SerializeField]
    Vector3 offset = Vector3.zero;
    [SerializeField]
    float lifeTime = 1.0f;
    
    protected override void OnStart()
    {
        base.OnStart();
        Transform target = ActorManager.I.GetTargetTransform(targetName, targetType);
        if (target == null)
        {
            Debug.LogError(target + "is not found");
            return;
        }
        
        SpriteAnimationManager.I.Play(spriteAnimationName, target.position + offset, lifeTime);
    }
}
