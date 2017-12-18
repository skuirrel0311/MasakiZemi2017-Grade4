using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : BaseStateMachineBehaviour
{
    public string objectName;

    protected override void OnStart()
    {
        base.OnStart();
        ActorManager.I.SetEnableObject(objectName, false);
    }
}
