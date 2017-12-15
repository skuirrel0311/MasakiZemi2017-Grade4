using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceHoloObject : BaseStateMachineBehaviour
{
    [SerializeField]
    string objectName = "";

    [SerializeField]
    string targetPointName = "";

    protected override void OnStart()
    {
        base.OnStart();

        HoloObject obj = ActorManager.I.GetObject(objectName);

        if(obj == null)
        {
            Debug.Log("not found " + objectName + " in instance holo object");
            return;
        }

        if(!string.IsNullOrEmpty(targetPointName))
        {
            Transform targetPoint = ActorManager.I.GetTargetPoint(targetPointName);

            if(targetPoint == null)
            {
                Debug.Log("not found " + targetPointName + " in instance holo object");
                return;
            }

            obj.transform.position = targetPoint.position;
            obj.transform.rotation = targetPoint.rotation;
        }

        obj.gameObject.SetActive(true);
    }
}
