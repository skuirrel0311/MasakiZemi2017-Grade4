using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsThereObject : Conditional
{
    public SharedGameObject targetObject = null;
    public SharedGameObject triggerObj = null;
    public LayerMask layer;
    MyTriggerBox trigger;

    public override void OnStart()
    {
        trigger = triggerObj.Value.GetComponent<MyTriggerBox>();
    }

    public override TaskStatus OnUpdate()
    {
        if (trigger == null)
        {
            Debug.Log("trigger is null");
            return TaskStatus.Failure;
        }
        return trigger.Intersect(targetObject.Value, layer) ? TaskStatus.Success : TaskStatus.Failure;
    }
}
