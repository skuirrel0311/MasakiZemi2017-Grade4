using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpItem : BaseStateMachineBehaviour
{
    [SerializeField]
    string targetObjectName = "";
    [SerializeField]
    string itemName = "";

    [SerializeField]
    bool isDrop = false;
    [SerializeField]
    bool isAll = false;
    
    protected override void OnStart()
    {
        base.OnStart();

        ActorManager actorManager = ActorManager.I;
        HoloObject target = actorManager.GetObject(targetObjectName);
        if(target == null)
        {
            Debug.LogError("not found " + targetObjectName + " in dump item");
            return;
        }

        if(isAll)
        {
            target.ItemSaucer.DumpItem(isDrop);
            return;
        }

        HoloItem item = actorManager.GetObject(itemName) as HoloItem;

        if(item == null)
        {
            Debug.LogError(targetObjectName + "is not found or don't item in dump item");
            return;
        }
        
        target.ItemSaucer.DumpItem(item, isDrop);
    }
}
