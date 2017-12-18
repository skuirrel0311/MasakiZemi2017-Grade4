using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : BaseStateMachineBehaviour
{
    [SerializeField]
    string[] wayPointNames = null;

    [SerializeField]
    float speed = 0.1f;

    Transform[] wayPoints;
    int currentIndex = 0;

    protected override void OnStart()
    {
        base.OnStart();

        wayPoints = new Transform[wayPointNames.Length];

        for (int i = 0; i < wayPointNames.Length; i++)
        {
            wayPoints[i] = ActorManager.I.GetTargetPoint(wayPointNames[i]);
        }
    }

    protected override BehaviourStatus OnUpdate()
    {


        return BehaviourStatus.Running;
    }
}
