using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyHasItem : HasItem
{
    [SerializeField]
    string[] targetItemNames = null;

    public override void SetFlag()
    {
        SetOwnerCharacter();

        bool hasItem = false;

        for (int i = 0; i < targetItemNames.Length; i++)
        {
            if (!OwnerHasItem(targetItemNames[i])) continue;
            //一つでもtrueだったら終了
            hasItem = true;
            break;
        }
        FlagManager.I.SetFlag(flagName, this, hasItem);
    }
}
