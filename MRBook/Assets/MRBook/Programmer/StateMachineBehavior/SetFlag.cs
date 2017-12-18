using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFlag : BaseStateMachineBehaviour
{
    [SerializeField]
    string flagName = "";
    [SerializeField]
    bool boolValue = true;

    protected override void OnStart()
    {
        base.OnStart();
        
        FlagManager.I.SetFlag(flagName, null, boolValue);
    }
}
