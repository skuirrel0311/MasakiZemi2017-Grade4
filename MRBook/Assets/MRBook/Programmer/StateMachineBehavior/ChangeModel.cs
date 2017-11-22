using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeModel : BaseStateMachineBehaviour
{
    [SerializeField]
    string objectName = "";

    protected override void OnStart()
    {
        base.OnStart();
        HoloObject obj = ActorManager.I.GetObject(objectName);
        if(obj == null)
        {
            Debug.LogWarning(objectName + "is null");
            return;
        }

        ChangeModelObject changeModelObject = obj.GetComponent<ChangeModelObject>();

        if(changeModelObject == null)
        {
            Debug.LogWarning(objectName + "is don't attach ChangeModelObject component");
            return;
        }

        changeModelObject.ChangeModel();
    }
}
