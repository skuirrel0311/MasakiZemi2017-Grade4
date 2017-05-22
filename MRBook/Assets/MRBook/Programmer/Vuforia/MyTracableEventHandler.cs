using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MyTracableEventHandler : MonoBehaviour,ITrackableEventHandler
{
    protected TrackableBehaviour m_TrackableBehaviour;

    protected virtual void Start()
    {
        m_TrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (m_TrackableBehaviour)
        {
            m_TrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }

    /// <summary>
    /// 見つけたらモデルを表示する
    /// </summary>
    protected virtual void OnTrackingFound()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
        
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }
        
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }
    }

    /// <summary>
    /// 見失ったら表示を中止する
    /// </summary>
    protected virtual void OnTrackingLost()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
        
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }
        foreach (Collider component in colliderComponents)
        {
            component.enabled = false;
        }
    }
}
