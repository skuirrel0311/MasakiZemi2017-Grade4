using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTotalResult : BaseStateMachineBehaviour
{
    protected override void OnStart()
    {
        base.OnStart();

        ResultManager.I.ShowTotalResult();
    }
}
