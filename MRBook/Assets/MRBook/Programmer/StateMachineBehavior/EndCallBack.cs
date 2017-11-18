using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCallBack : BaseStateMachineBehaviour
{
    public bool success = false;

    protected override void OnStart()
    {
        base.OnStart();
        MainSceneManager.I.EndCallBack(success);
    }
}
