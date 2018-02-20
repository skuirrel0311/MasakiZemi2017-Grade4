using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoloObjectController : MyObjPositionController
{
    [SerializeField]
    protected LayerMask layerMask = 1 << 8;
    public Action OnItemDragStart;
    Vector3 offset;

    public bool canClick;
    Transform oldParent;

    #region シングルトン
    static HoloObjectController instance;
    public static HoloObjectController I
    {
        get
        {
            if (instance == null)
            {
                instance = (HoloObjectController)FindObjectOfType(typeof(HoloObjectController));
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
        List<HoloObjectController> instances = new List<HoloObjectController>();
        instances.AddRange((HoloObjectController[])FindObjectsOfType(typeof(HoloObjectController)));

        if (I == null) I = instances[0];
        instances.Remove(I);

        if (instances.Count == 0) return;
        //あぶれ者のinstanceはデストロイ 
        foreach (HoloObjectController t in instances) Destroy(t.gameObject);
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= WasLoaded;
        I = null;
    }
    #endregion

    protected override void Start()
    {
        canClick = true;

        base.Start();
    }
    
    public virtual void SetTargetObject(HoloObject obj)
    {
        Debug.Log("input down in ObjCon");
        oldParent = obj.transform.parent;
        targetObject = obj.gameObject;
        transform.position = targetObject.transform.position;
        
        obj.transform.parent = transform;
    }

    public virtual void Disable(bool setParent = true)
    {
        if (targetObject != null && setParent)
        {
            targetObject.transform.parent = oldParent;
        }
        targetObject = null;
    }
}
