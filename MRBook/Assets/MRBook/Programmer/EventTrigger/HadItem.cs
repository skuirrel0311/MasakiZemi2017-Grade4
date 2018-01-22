using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// そのアイテムが持たれているか？
/// </summary>
public class HadItem : MyEventTrigger
{
    [SerializeField]
    HoloItem item = null; 

    public override void SetFlag()
    {
        if(item == null)
        {
            FlagManager.I.SetFlag(flagName, this, false);
            return;
        }
        FlagManager.I.SetFlag(flagName, this, item.owner != null);
    }

}
