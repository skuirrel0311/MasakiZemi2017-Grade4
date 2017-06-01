using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class HoloTracableEventHandler : MyTracableEventHandler
{
    [SerializeField]
    GameObject[] objArray = null;
    bool isView = false;

    [SerializeField]
    Transform anchorTransform = null;

    protected override void OnTrackingFound()
    {
        //一回のみ生成
        if (isView) return;
        isView = true;
        Vector3 pos;
        //WorldAnchorの位置に生成する
        for (int i = 0; i < objArray.Length; i++)
        {
            objArray[i].SetActive(true);
            pos = anchorTransform.position + objArray[i].transform.position;
            objArray[i].transform.position = pos + (Vector3.up * -0.1f);
            objArray[i].transform.rotation = anchorTransform.rotation;
        }
        VuforiaBehaviour.Instance.enabled = false;
    }

    protected override void OnTrackingLost() { }
}
