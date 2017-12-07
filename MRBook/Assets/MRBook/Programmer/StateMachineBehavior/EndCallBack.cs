using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCallBack : BaseStateMachineBehaviour
{
    public bool success = false;

    protected override void OnStart()
    {
        base.OnStart();
        m_animator.SetBool("IsStart", false);
        PageResultManager.I.SetResult(success);
        PageResultManager.I.ShowResult();
    }
}
