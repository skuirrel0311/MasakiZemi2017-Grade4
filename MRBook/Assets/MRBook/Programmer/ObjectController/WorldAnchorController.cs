using UnityEngine;
using UnityEngine.VR.WSA;

/// <summary>
/// キューブ型のWorldAnchorを移動させたり固定させたりするもの
/// </summary>
public class WorldAnchorController : HoloObject
{
    protected MyWorldAnchorManager worldAnchorManager;

    Color movableColor = Color.yellow;
    Color staticColor = Color.green;

    BoxCollider m_collider;
    Renderer m_renderer;
    MaterialPropertyBlock block;

    /// <summary>
    /// BoundingBoxでの操作を受け付けるか？
    /// </summary>
    bool isObserver = false;
    bool canDragging = false;

    protected virtual void Start()
    {
        m_collider = GetComponent<BoxCollider>();
        m_renderer = GetComponent<Renderer>();
        worldAnchorManager = MyWorldAnchorManager.I;
        block = new MaterialPropertyBlock();

        LoadAnchor();
        ChangeObserverState(true);

        Init();
    }

    void ChangeObserverState(bool isObserver)
    {
        if (this.isObserver == isObserver) return;

        this.isObserver = isObserver;

        if (this.isObserver)
            MyObjControllerByBoundingBox.I.OnTargetChanged += OnTargetChangedInObjCon;
        else
            MyObjControllerByBoundingBox.I.OnTargetChanged -= OnTargetChangedInObjCon;
    }
    
    void OnTargetChangedInObjCon(GameObject oldObj, GameObject newObj)
    {
        bool equalOld = gameObject.Equals(oldObj);
        bool equalNew = gameObject.Equals(newObj);

        if (equalOld && !equalNew) OnBreak();
        else if (equalNew) OnClicked();
    }

    //このオブジェクトを選択した状態で別のオブジェクトを選択した
    void OnBreak()
    {
        canDragging = false;
        SaveAnchor();
    }

    void OnClicked()
    {
        canDragging = !canDragging;
        if (canDragging)
            DeleteAnchor();
        else
            SaveAnchor();
    }

    public virtual void SaveAnchor()
    {
        worldAnchorManager.SaveAnchor(gameObject);
        SetColor(staticColor);
    }

    public virtual void DeleteAnchor()
    {
        worldAnchorManager.DeleteAnchor(gameObject);
        SetColor(movableColor);
    }

    public virtual void LoadAnchor()
    {
        worldAnchorManager.LoadAnchor(gameObject, (anchor) =>
        {
            SetColor(anchor != null ? staticColor : movableColor);
        });
    }

    void SetColor(Color col)
    {
        block.SetColor("_Color", col);

        m_renderer.SetPropertyBlock(block);
    }

    /// <summary>
    /// 表示とコリジョンをon,offします
    /// </summary>
    /// <param name="isActive"></param>
    public virtual void SetActive(bool isActive)
    {
        m_renderer.enabled = isActive;
        m_collider.enabled = isActive;
        ChangeObserverState(isActive);
    }
}
