using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

public class HoloTracableEventHandler : MyTracableEventHandler
{
    [SerializeField]
    GameObject[] objArray = null;
    bool isView = false;

    [SerializeField]
    Transform anchorTransform = null;

    [SerializeField]
    Vector3 offset = new Vector3(0.0f, -0.1f, 0.0f);

    WorldAnchorStore anchorStore;

    protected override void Start()
    {
        base.Start();
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    protected override void OnTrackingFound()
    {
        //一回のみ生成
        if (isView) return;
        isView = true;
        Vector3 pos;
        //WorldAnchorの位置に移動
        for (int i = 0; i < objArray.Length; i++)
        {
            objArray[i].SetActive(true);
            pos = anchorTransform.position + objArray[i].transform.position;
            objArray[i].transform.position = pos + offset;
            objArray[i].transform.rotation = anchorTransform.rotation;
            AttachingAnchor(objArray[i].transform.Find("stage").gameObject);
        }
        VuforiaBehaviour.Instance.enabled = false;
    }

    void AnchorStoreReady(WorldAnchorStore anchorStore)
    {
        this.anchorStore = anchorStore;
    }

    //objにWorldAnchorをアタッチする
    void AttachingAnchor(GameObject obj)
    {
        WorldAnchor attaching = obj.AddComponent<WorldAnchor>();
        attaching.OnTrackingChanged += AttachingAnchor_OnTrackingChanged;
    }
    void AttachingAnchor_OnTrackingChanged(WorldAnchor self, bool located)
    {
        if (!located) return;

        anchorStore.Save(self.gameObject.name, self);

        self.OnTrackingChanged -= AttachingAnchor_OnTrackingChanged;
    }

    protected override void OnTrackingLost() { }
}
