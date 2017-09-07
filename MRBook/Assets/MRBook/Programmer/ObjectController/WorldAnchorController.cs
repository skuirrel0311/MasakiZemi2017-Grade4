using UnityEngine;
using UnityEngine.VR.WSA;

//キューブ型のWorldAnchorを移動させたり固定させたりするもの
public class WorldAnchorController : HoloMovableObject
{
    protected MyWorldAnchorManager worldAnchorManager;

    Color movableColor = Color.yellow;
    Color staticColor = Color.green;

    Renderer m_renderer;
    MaterialPropertyBlock block;

    /// <summary>
    /// BoundingBoxでの操作を監視するか？
    /// </summary>
    bool isObserver = false;
    bool canDragging = false;
    protected bool isLoaded = false;

    protected virtual void Start()
    {
        m_renderer = GetComponent<Renderer>();
        worldAnchorManager = MyWorldAnchorManager.I;
        block = new MaterialPropertyBlock();

        worldAnchorManager.AddOnLoadedAction((store) =>
        {
            isLoaded = true;
            SaveAnchor();
        });

        ChangeObserverState(true);
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
        if (!isLoaded) return;
        worldAnchorManager.SaveAnchor(gameObject);
        SetColor(staticColor);
    }

    public virtual void DeleteAnchor()
    {
        if (!isLoaded) return;
        worldAnchorManager.anchorStore.Delete(name);
        DestroyImmediate(GetComponent<WorldAnchor>());
        SetColor(movableColor);
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
