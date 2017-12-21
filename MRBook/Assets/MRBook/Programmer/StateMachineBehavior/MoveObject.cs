using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : BaseStateMachineBehaviour
{
    [SerializeField]
    string objectName = "";

    [SerializeField]
    string[] wayPointNames = null;

    [SerializeField]
    float speed = 0.1f;

    HoloObject targetObject;
    Transform[] wayPoints;
    int currentIndex = 0;

    float t;

    Vector3 startPosition;
    Vector3 targetPosition;
    //startからtargetまで行くのにかかる時間
    float targetTime;

    protected override void OnStart()
    {
        base.OnStart();

        t = 0.0f;
        currentIndex = 0;

        targetObject = ActorManager.I.GetObject(objectName);

        wayPoints = new Transform[wayPointNames.Length];

        for (int i = 0; i < wayPointNames.Length; i++)
        {
            wayPoints[i] = ActorManager.I.GetTargetPoint(wayPointNames[i]);
        }

        SetTargetPoint(currentIndex);
    }

    void SetTargetPoint(int index)
    {
        startPosition = targetObject.transform.position;
        targetPosition = wayPoints[index].position;

        float distance = (targetPosition - startPosition).magnitude;

        targetTime = distance / speed;
    }

    protected override BehaviourStatus OnUpdate()
    {
        t += Time.deltaTime;

        float progress = t / targetTime;
        targetObject.transform.position = Vector3.Lerp(startPosition, targetPosition, progress);

        if(progress >= 1.0f)
        {
            currentIndex++;
            if (currentIndex >= wayPoints.Length) return BehaviourStatus.Success;
            t = 0.0f;
            SetTargetPoint(currentIndex);
        }

        return BehaviourStatus.Running;
    }
}
