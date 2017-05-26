using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloTracableEventHandler : MyTracableEventHandler
{
    [SerializeField]
    GameObject obj = null;
    bool isView = false;

    [SerializeField]
    Transform anchorTransform = null;

    protected override void OnTrackingFound()
    {
        //一回のみ生成
        if (isView) return;
        isView = true;

        //WorldAnchorの位置に生成する
        Instantiate(obj, anchorTransform.position, anchorTransform.rotation);
    }

    protected override void OnTrackingLost() { }
}
