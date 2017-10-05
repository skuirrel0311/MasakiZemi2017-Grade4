using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToStartMessage : HoloButton
{
    [SerializeField]
    Transform targetTransform = null;

    protected override void Update()
    {
        base.Update();
        //完全追従
        transform.SetPositionAndRotation(targetTransform.position, targetTransform.rotation);
    }
}
