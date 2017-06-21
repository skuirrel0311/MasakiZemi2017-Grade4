using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

public class MyWorldAnchorManager : BaseManager<MyWorldAnchorManager>
{
    public WorldAnchorStore anchorStore;

    protected override void Start()
    {
        base.Start();
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    void AnchorStoreReady(WorldAnchorStore anchorStore)
    {
        this.anchorStore = anchorStore;
    }

    public void AttachingAnchor(GameObject obj)
    {
        WorldAnchor attaching = obj.AddComponent<WorldAnchor>();
        attaching.OnTrackingChanged += AttachingAnchor_OnTrackingChanged;
    }

    public IEnumerator GetAnchorStore(WorldAnchorStore anchorStore, Action action)
    {
        yield return null;

        while(this.anchorStore == null)
        {
            yield return null;
        }

        //ロードされた自身のAnchorStoreを入れる
        anchorStore = this.anchorStore;

        action.Invoke();
    }

    void AttachingAnchor_OnTrackingChanged(WorldAnchor self, bool located)
    {
        if (!located) return;

        anchorStore.Save(self.gameObject.name, self);

        self.OnTrackingChanged -= AttachingAnchor_OnTrackingChanged;
    }
}
