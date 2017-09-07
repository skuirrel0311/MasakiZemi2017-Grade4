using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

public class MyWorldAnchorManager : BaseManager<MyWorldAnchorManager>
{
    public WorldAnchorStore anchorStore;

    Action<WorldAnchorStore> onLoaded;

    protected override void Start()
    {
        base.Start();
        WorldAnchorStore.GetAsync(AnchorStoreReady);
        WorldManager.OnPositionalLocatorStateChanged += WorldManagerOnStateChanged;
    }

    void AnchorStoreReady(WorldAnchorStore anchorStore)
    {
        this.anchorStore = anchorStore;
        if(onLoaded != null)  onLoaded.Invoke(anchorStore);
    }

    public void SaveAnchor(GameObject obj)
    {
        WorldAnchor attaching = obj.AddComponent<WorldAnchor>();
        attaching.OnTrackingChanged += AttachingAnchor_OnTrackingChanged;
    }

    public WorldAnchor LoadAnchor(GameObject obj)
    {
        return anchorStore.Load(obj.name, obj);
    }

    public IEnumerator GetAnchorStore(Action<WorldAnchorStore> action)
    {
        yield return null;

        while(anchorStore == null)
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

    public void AddOnLoadedAction(Action<WorldAnchorStore> act)
    {
        if(anchorStore != null)
        {
            act.Invoke(anchorStore);
            return;
        }

        onLoaded += act;
    }

    void WorldManagerOnStateChanged(PositionalLocatorState oldState, PositionalLocatorState newState)
    {
        if(newState == PositionalLocatorState.Active)
        {
            MyGameManager.I.ModifiBookPosition();
        }
    }
}
