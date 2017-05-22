using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

public class AnchorPositionController : MyObjPositionController, IInputClickHandler
{
    Color startColor;
    //動かせるか？
    bool isMovable = false;

    Renderer m_rendere;

    WorldAnchorStore anchorStore;

    protected override void Start()
    {
        base.Start();
        m_rendere = GetComponent<Renderer>();
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    void AnchorStoreReady(WorldAnchorStore anchorStore)
    {
        this.anchorStore = anchorStore;

        WorldAnchor attached = anchorStore.Load(name, gameObject);

        if (attached == null)
        {
            anchorStore.Save(name, GetComponent<WorldAnchor>());
            m_rendere.material.color = Color.red;
        }
        else
        {
            m_rendere.material.color = Color.blue;
        }

        startColor = m_rendere.material.color;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        isMovable = !isMovable;

        if (isMovable)
        {
            anchorStore.Delete(name);
            DestroyImmediate(GetComponent<WorldAnchor>());
            m_rendere.material.color = Color.green;
        }
        else
        {
            //即座にアタッチはされない
            WorldAnchor attaching = gameObject.AddComponent<WorldAnchor>();
            attaching.OnTrackingChanged += AttachingAnchor_OnTrackingChanged;
            m_rendere.material.color = startColor;
        }
    }

    void AttachingAnchor_OnTrackingChanged(WorldAnchor self, bool located)
    {
        if (!located) return;

        anchorStore.Save(name, self);

        self.OnTrackingChanged -= AttachingAnchor_OnTrackingChanged;
    }
}
