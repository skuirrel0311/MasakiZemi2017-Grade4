using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//バウンディングボックスを用いたオブジェクトの操作
public class MyObjControllerByBoundingBox : MyObjPositionController
{
    public bool canDragging { get; private set; }
    Renderer[] m_renderers;
    Collider[] m_cols;
    Transform oldParent = null;
    
    Vector3 offset;
    [SerializeField]
    LayerMask layerMask = 1 << 8;

    public Action<GameObject, GameObject> OnTargetChanged;

    //シングルトン todo: インターフェース化できるとベスト
    static MyObjControllerByBoundingBox instance;
    public static MyObjControllerByBoundingBox I
    {
        get
        {
            if (instance == null)
            {
                instance = (MainSceneObjController)FindObjectOfType(typeof(MainSceneObjController));
            }
            return instance;
        }
        protected set
        {
            instance = value;
        }
    }
    protected virtual void Awake()
    {
        Inisialize();
        SceneManager.sceneLoaded += WasLoaded;
    }
    void WasLoaded(Scene scneName, LoadSceneMode sceneMode)
    {
        Inisialize();
    }
    void Inisialize()
    {
        List<MyObjControllerByBoundingBox> instances = new List<MyObjControllerByBoundingBox>();
        instances.AddRange((MyObjControllerByBoundingBox[])FindObjectsOfType(typeof(MyObjControllerByBoundingBox)));

        if (I == null) I = instances[0];
        instances.Remove(I);

        if (instances.Count == 0) return;
        //あぶれ者のinstanceはデストロイ 
        foreach (MyObjControllerByBoundingBox t in instances) Destroy(t.gameObject);
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= WasLoaded;
        I = null;
    }

    protected override void Start()
    {
        m_renderers = GetComponentsInChildren<Renderer>();
        m_cols = GetComponentsInChildren<Collider>();
        ChangeWireFrameView(false);
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (canDragging && !isDragging)
        {
            Vector3 actorPosition = targetObject.transform.position;
            transform.position = actorPosition + offset;

            targetObject.transform.position = actorPosition;
        }
    }

    public virtual void SetTargetObject(GameObject obj)
    {
        if (OnTargetChanged != null) OnTargetChanged(targetObject, obj);

        Debug.Log("call set target object");
        if (targetObject != null && targetObject.Equals(obj))
        {
            Disable();
            return;
        }

        if (targetObject != null)
        {
            targetObject.transform.parent = oldParent;
        }
        //targetObjectの切り替え
        targetObject = obj;

        transform.position = targetObject.transform.position;

        BoxCollider boxCol = targetObject.GetComponent<BoxCollider>();

        Vector3 boxSize = targetObject.transform.lossyScale;
        boxSize.x *= boxCol.size.x;
        boxSize.y *= boxCol.size.y;
        boxSize.z *= boxCol.size.z;

        transform.localScale = boxSize;

        offset = targetObject.transform.lossyScale;
        offset.x *= boxCol.center.x;
        offset.y *= boxCol.center.y;
        offset.z *= boxCol.center.z;

        transform.position += offset;
        transform.rotation = targetObject.transform.rotation;

        ChangeWireFrameView(true);

        oldParent = obj.transform.parent;
        obj.transform.parent = transform;
    }

    protected override void StartDragging()
    {
        if (!canDragging) return;

        base.StartDragging();
    }

    public void Disable()
    {
        if (targetObject != null)
        {
            targetObject.transform.parent = oldParent;
        }
        targetObject = null;

        ChangeWireFrameView(false);
    }

    public void ChangeWireFrameView(bool canDragging)
    {
        this.canDragging = canDragging;

        for (int i = 0; i < m_renderers.Length; i++)
        {
            m_renderers[i].enabled = canDragging;
        }

        for (int i = 0; i < m_cols.Length; i++)
        {
            m_cols[i].enabled = canDragging;
        }
    }
}
