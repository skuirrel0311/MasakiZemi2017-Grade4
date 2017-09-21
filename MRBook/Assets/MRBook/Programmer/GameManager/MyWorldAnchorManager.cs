using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

/// <summary>
/// WorldAnchorのアタッチや削除をやってくれる人
/// </summary>
public class MyWorldAnchorManager : BaseManager<MyWorldAnchorManager>
{
    public WorldAnchorStore anchorStore;

    Action<WorldAnchorStore> onLoaded;

    protected override void Start()
    {
        base.Start();
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    void AnchorStoreReady(WorldAnchorStore anchorStore)
    {
        this.anchorStore = anchorStore;
        if (onLoaded != null) onLoaded.Invoke(anchorStore);
    }

    public void SaveAnchor(GameObject obj)
    {
        WorldAnchor attaching = obj.AddComponent<WorldAnchor>();
        attaching.OnTrackingChanged += AttachingAnchor_OnTrackingChanged;
    }

    public void LoadAnchor(GameObject obj, Action<WorldAnchor> onLoaded = null)
    {
        FutureAnchorStore((store) =>
        {
            WorldAnchor anchor = store.Load(obj.name, obj);
            if(onLoaded != null) onLoaded.Invoke(anchor);
        });
    }

    public void DeleteAnchor(GameObject obj)
    {
        FutureAnchorStore((store) =>
        {
            store.Delete(obj.name);
            DestroyImmediate(obj.GetComponent<WorldAnchor>());
        });
    }

    public IEnumerator GetAnchorStore(Action<WorldAnchorStore> action)
    {
        yield return null;

        while (anchorStore == null)
        {
            yield return null;
        }

        action.Invoke(anchorStore);
    }

    void AttachingAnchor_OnTrackingChanged(WorldAnchor self, bool located)
    {
        if (!located) return;

        anchorStore.Save(self.gameObject.name, self);

        self.OnTrackingChanged -= AttachingAnchor_OnTrackingChanged;
    }

    /// <summary>
    /// AnchorStoreをロードし終わったらactionにAnchorStoreを渡す
    /// </summary>
    public void FutureAnchorStore(Action<WorldAnchorStore> action)
    {
        if (anchorStore != null)
        {
            action.Invoke(anchorStore);
            return;
        }

        onLoaded += action;
    }
}
