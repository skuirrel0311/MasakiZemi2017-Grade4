using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildEndPoint : BaseStateMachineBehaviour
{
    public override TaskType GetTaskType { get { return TaskType.EndPoint; } }
}
