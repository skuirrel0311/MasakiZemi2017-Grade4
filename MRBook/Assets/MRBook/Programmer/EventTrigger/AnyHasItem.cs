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

        for (int i = 0; i < targetItemNames.Length; i++)
        {
            if (!OwnerHasItem(targetItemNames[i])) continue;
            //一つでもtrueだったら終了
            FlagManager.I.SetFlag(flagName, this, true);
            return;
        }
        FlagManager.I.SetFlag(flagName, this, false);
    }
}
