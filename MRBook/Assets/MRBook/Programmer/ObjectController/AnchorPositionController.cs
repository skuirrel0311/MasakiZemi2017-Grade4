using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

public class AnchorPositionController : MyObjPositionController, IInputClickHandler
{
    Color startColor = Color.red;
    //動かせるか？
    public bool IsMovable { get; private set; }

    Renderer m_rendere;

    MyWorldAnchorManager anchorSroreManager;
    WorldAnchorStore anchorStore;

    protected override void Start()
    {
        base.Start();
        m_rendere = GetComponent<Renderer>();

        anchorSroreManager = MyWorldAnchorManager.I;

        anchorStore = null;

        Debug.Log("start coroutine");
        StartCoroutine(anchorSroreManager.GetAnchorStore((anchorStore) =>
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
            m_rendere.material.color = Color.red;
        }
        else
        {
            //ここのコメントアウトを切れば初回のみアンカーが動かせる状態にできる
            //gameObject.SetActive(false);

            m_rendere.material.color = Color.blue;
        }

        startColor = m_rendere.material.color;
    }

    protected override void Update()
    {
        base.Update();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (anchorStore == null) return;

        IsMovable = !IsMovable;

        if (IsMovable)
        {
            anchorSroreManager.anchorStore.Delete(name);
            DestroyImmediate(GetComponent<WorldAnchor>());
            m_rendere.material.color = Color.green;
        }
        else
        {
            anchorSroreManager.AttachingAnchor(gameObject);
            m_rendere.material.color = startColor;
        }
    }

    public void Hide()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        for(int i= 0;i< renderers.Length;i++)
        {
            renderers[i].enabled = false;
        }

        Collider[] cols = GetComponentsInChildren<Collider>();
        for(int i = 0;i < cols.Length;i++)
        {
            cols[i].enabled = false;
        }
    }
}
