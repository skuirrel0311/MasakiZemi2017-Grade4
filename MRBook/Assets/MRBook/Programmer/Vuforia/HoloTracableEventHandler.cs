using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloTracableEventHandler : MyTracableEventHandler
{
    [SerializeField]
    GameObject obj = null;
    bool isView = false;

    protected override void OnTrackingFound()
    {
        //一回のみ生成
        if (isView) return;
        isView = true;
        Instantiate(obj, transform.position, transform.rotation);
    }

    protected override void OnTrackingLost()
    {
    }
}
