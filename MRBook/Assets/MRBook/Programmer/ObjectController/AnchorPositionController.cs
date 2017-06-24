using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

public class AnchorPositionController : MyObjPositionController, IInputClickHandler
{
    Color startColor;
    //動かせるか？
    public bool IsMovable { get; private set; }

    Renderer m_rendere;

    MyWorldAnchorManager anchorSroreManager;

    protected override void Start()
    {
        base.Start();
        m_rendere = GetComponent<Renderer>();

        anchorSroreManager = MyWorldAnchorManager.I;

        WorldAnchorStore anchorStore = null;
        MyWorldAnchorManager.I.GetAnchorStore(anchorStore, () =>
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
        });
    }

    protected override void Update()
    {
        base.Update();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
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
}
