using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTarget : StateMachineBehaviour
{
    public string targetName;
    public string actorName;
    public float rotationSpeed;
    protected Transform target;
    protected Transform actor;

    Quaternion from, to;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        target = ActorManager.I.GetTarget(targetName);
        actor = ActorManager.I.GetActor(actorName).transform;
        Vector3 targetDirection = target.position - actor.position;
        
        from = actor.rotation;
        to = Quaternion.LookRotation(targetDirection);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        target.rotation = Quaternion.RotateTowards(from, to, rotationSpeed * Time.deltaTime);

        if(Quaternion.Angle(target.rotation, to) < 0.5f)
        {
            //回転し終わった
            animator.SetBool("IsCompleted", true);
        }
    }
}
