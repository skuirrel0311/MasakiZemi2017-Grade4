using UnityEngine;

public class MainSceneObjController : MyObjControllerByBoundingBox
{
    protected ActorManager actorManager;
    //掴んでいるアクター
    protected HoloObject targetMovableObject;

    //アイテムを掴んでいるか？
    protected bool isHoldItem = false;

    protected Ray ray;
    [SerializeField]
    LayerMask ignoreLayerMask = 1 << 2;

    [SerializeField]
    UnderTargetMaker underTargetMaker = null;

    //キャッシュ
    Vector3 downVec = Vector3.down;

    const int bookLayer = 11;

    float maxDistance;

    protected override void Start()
    {
        base.Start();
        actorManager = ActorManager.I;

        if (MainSceneManager.I == null) return;
        MainSceneManager.I.OnPlayPage += (page) =>
        {
            Disable();
        };
        MainSceneManager.I.OnReset += () => Disable();
    }

    protected override void StartDragging()
    {
        if (!canDragging) return;

        StartOperation();

        base.StartDragging();
    }
    protected override void UpdateDragging()
    {
        base.UpdateDragging();
        UpdateOperation();
    }
    protected override void StopDragging()
    {
        base.StopDragging();
        EndOperation();
    }

    /// <summary>
    /// 呼ばれる前にtargetObjにはセットされている
    /// </summary>
    protected virtual void StartOperation()
    {
        MainSceneManager sceneManager = MainSceneManager.I;
        maxDistance = targetMovableObject.transform.position.y - sceneManager.pages[sceneManager.currentPageIndex].transform.position.y + 0.5f;
        if(targetMovableObject.GetActorType == HoloObject.Type.Item)
        {
            isHoldItem = true;
            if(OnItemDragStart != null) OnItemDragStart.Invoke();
        }
        targetMovableObject.InputHandler.OnDragStart();
    }

    protected virtual void UpdateOperation()
    {
        RaycastHit underObj;
        bool isHitObject = TryGetUnderObject(out underObj);
        HoloObject hitObj = GetHitHoloObject(underObj, isHitObject);
        BaseObjInputHandler.HitObjType hitObjType = GetHitObjType(underObj, isHitObject);

        BaseObjInputHandler.MakerType makerType = targetMovableObject.InputHandler.OnDragUpdate(hitObjType, hitObj);

        //Debug.Log("makerType = " + makerType.ToString());
        underTargetMaker.SetMaker(makerType, targetMovableObject, underObj);
    }

    protected virtual void EndOperation()
    {
        RaycastHit underObj;
        bool isHitObject = TryGetUnderObject(out underObj);
        HoloObject hitObj = GetHitHoloObject(underObj, isHitObject);
        BaseObjInputHandler.HitObjType hitObjType = GetHitObjType(underObj, isHitObject);

        targetMovableObject.InputHandler.OnDragEnd(hitObjType, hitObj);

        Disable();

        underTargetMaker.HideMaker();
        
        if(isHoldItem)
        {
            isHoldItem = false;
            if(OnItemDragEnd != null) OnItemDragEnd.Invoke();
        }
    }
    
    HoloObject GetHitHoloObject(RaycastHit hit, bool isHit)
    {
        if (!isHit) return null;
        
        if (hit.transform.gameObject.layer == bookLayer) return null;
        
        return hit.transform.GetComponent<HoloObject>();
    }

    BaseObjInputHandler.HitObjType GetHitObjType(RaycastHit hit, bool isHit)
    {
        if (!isHit) return BaseObjInputHandler.HitObjType.None;

        if (hit.transform.gameObject.layer == bookLayer) return BaseObjInputHandler.HitObjType.Book;

        HoloObject obj = hit.transform.GetComponent<HoloObject>();

        if (obj != null && obj.GetActorType == HoloObject.Type.Character)return BaseObjInputHandler.HitObjType.Character;

        return BaseObjInputHandler.HitObjType.OtherObj;
    }
    
    /// <summary>
    /// 渡されたオブジェクトに合わせてバウンディングボックスを表示し、操作できるようにする
    /// </summary>
    /// <param name="obj"></param>
    public override void SetTargetObject(HoloObject obj)
    {
        SetTargetObject(obj.gameObject);

        targetMovableObject = obj;
    }

    /// <summary>
    /// 掴んでいるオブジェクトの直下のオブジェクトを取得する
    /// </summary>
    bool TryGetUnderObject(out RaycastHit hitObj)
    {
        //直下を調べる
        ray.direction = Vector3.down;
        ray.origin = targetMovableObject.transform.position;

        RaycastHit[] hits = Physics.SphereCastAll(ray, targetMovableObject.InputHandler.SphereCastRadius, this.maxDistance, ~ignoreLayerMask);
        hitObj = new RaycastHit();
        bool isHit = false;
        float maxDistance = this.maxDistance;

        for (int i = 0; i < hits.Length; i++)
        {
            //こちらでオーバライドしたEqualsを呼び出している(アイテムを持っている時の対応)
            if (targetMovableObject.Equals(hits[i].transform.gameObject)) continue;

            isHit = true;
            //自身以外に当たった

            //一番距離が近いやつが直下のオブジェクト
            if (hits[i].distance < maxDistance)
            {
                hitObj = hits[i];
                maxDistance = hits[i].distance;
            }
        }

        return isHit;
    }
}
