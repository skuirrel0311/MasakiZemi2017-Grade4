using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

public class WorldAnchorController : MyObjPositionController, IInputClickHandler
{
    bool isMovable = false;
    Renderer m_renderer;
    MyWorldAnchorManager worldAnchorManager;
    WorldAnchorStore anchorStore;
    
    Color movableColor = Color.green;
    Color staticColor = Color.gray;

    protected override void Start()
    {
        base.Start();
        m_renderer = GetComponent<Renderer>();
        worldAnchorManager = MyWorldAnchorManager.I;
        anchorStore = null;
        
        StartCoroutine(worldAnchorManager.GetAnchorStore((anchorStore) =>
        {
            this.anchorStore = anchorStore;
            StartUpAnchor();
        }));
    }

    void StartUpAnchor()
    {
        WorldAnchor attached = anchorStore.Load(name, gameObject);

        if (attached == null)
        {
            anchorStore.Save(name, GetComponent<WorldAnchor>());
            m_renderer.material.color = Color.red;
        }
        else
        {
            m_renderer.material.color = Color.blue;
        }
        
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        isMovable = !isMovable;

        if (m_renderer == null) return;

        if (isMovable)
        {
            worldAnchorManager.anchorStore.Delete(name);
            DestroyImmediate(GetComponent<WorldAnchor>());
        }
        else
        {
            worldAnchorManager.AttachingAnchor(gameObject);
        }
    }
}
