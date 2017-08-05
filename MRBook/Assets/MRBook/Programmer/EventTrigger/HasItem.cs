using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasItem : MyEventTrigger
{
    public override void SetFlag()
    {
        HoloItem item = targetObject.GetComponent<HoloItem>();
        if (item == null || item.owner == null)
        {
            return;
        }

        FlagManager.I.SetFlag(flagName, this, transform.parent.Equals(item.owner.transform));
    }
}
