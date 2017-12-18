using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSecretBoxContentKind : BaseStateMachineBehaviour
{
    protected override void OnStart()
    {
        base.OnStart();
        m_animator.SetInteger("ContentKind", ResultManager.I.GetContentKind());
    }
}
