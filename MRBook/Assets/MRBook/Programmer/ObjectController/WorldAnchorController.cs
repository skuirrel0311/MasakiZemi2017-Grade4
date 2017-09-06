using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

//キューブ型のWorldAnchorを移動させたり固定させたりするもの
public class WorldAnchorController : HoloMovableObject
{
    MyWorldAnchorManager worldAnchorManager;

    Color movableColor = Color.yellow;
    Color staticColor = Color.green;

    Renderer m_renderer;
    BoxCollider m_collider;
    MaterialPropertyBlock block;

    public bool isObserver = false;
    public bool canDragging = false;
    public bool isLoaded = false;

    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        m_collider = GetComponent<BoxCollider>();
        worldAnchorManager = MyWorldAnchorManager.I;
        block = new MaterialPropertyBlock();

        worldAnchorManager.AddOnLoadedAction((store) =>
        {
            isLoaded = true;
            SetColor(staticColor);
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

        if (equalNew) OnClicked();
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

    public void SaveAnchor()
    {
        if (!isLoaded) return;
        worldAnchorManager.SaveAnchor(gameObject);
        SetColor(staticColor);
    }

    public void DeleteAnchor()
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

    public void SetActive(bool isActive)
    {
        m_renderer.enabled = isActive;
        m_collider.enabled = isActive;
        ChangeObserverState(isActive);
    }
}
