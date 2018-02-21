using UnityEngine;

public class MainSceneObjController : HoloObjectController
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
    const int eventAreaLayer = 13;

    float maxDistance;

    BaseObjInputHandler.MakerType oldMakerType;
    
    protected override void Start()
    {
        base.Start();
        actorManager = ActorManager.I;
        MainSceneManager sceneManager = MainSceneManager.I;

        if (sceneManager == null) return;
        sceneManager.OnPlayPage += () =>
        {
            canClick = false;
            Disable();
        };
        sceneManager.OnReset += () =>
        {
            canClick = true;
            Disable();
        };
        sceneManager.OnPageLoaded += (page) =>
        {
            canClick = true;
        };

    }

    protected override void StartDragging()
    {
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
        maxDistance = targetMovableObject.transform.position.y - OffsetController.I.bookTransform.position.y + 0.5f;
        if (targetMovableObject.GetActorType == HoloObject.Type.Item)
        {
            isHoldItem = true;
            if (OnItemDragStart != null) OnItemDragStart.Invoke();
        }
        targetMovableObject.InputHandler.OnDragStart();

        RaycastHit underObj;
        bool isHitObject = TryGetUnderObject(out underObj);
        underTargetMaker.InitializeMaker(targetMovableObject, underObj, GetMakerType(underObj, isHitObject));

        oldMakerType = BaseObjInputHandler.MakerType.None;
    }

    protected virtual void UpdateOperation()
    {
        RaycastHit underObj;
        bool isHitObject = TryGetUnderObject(out underObj);

        BaseObjInputHandler.MakerType makerType = GetMakerType(underObj, isHitObject);

        //Debug.Log("makerType = " + makerType.ToString());
        underTargetMaker.UpdateMaker(makerType, underObj);
        oldMakerType = makerType;
    }

    BaseObjInputHandler.MakerType GetMakerType(RaycastHit underObj, bool isHitObject)
    {
        HoloObject hitObj = GetHitHoloObject(underObj, isHitObject);
        BaseObjInputHandler.HitObjType hitObjType = GetHitObjType(underObj, isHitObject);
        return targetMovableObject.InputHandler.OnDragUpdate(hitObjType, hitObj);
    }

    protected virtual void EndOperation()
    {
        RaycastHit underObj;
        bool isHitObject = TryGetUnderObject(out underObj);
        HoloObject hitObj = GetHitHoloObject(underObj, isHitObject);
        BaseObjInputHandler.HitObjType hitObjType = GetHitObjType(underObj, isHitObject);
        BaseObjInputHandler.MakerType makerType = targetMovableObject.InputHandler.OnDragUpdate(hitObjType, hitObj);
        
        targetMovableObject.InputHandler.OnDragEnd(hitObjType, hitObj);
        underTargetMaker.SetMakerEnable(false);
        isHoldItem = false;
        bool changeParent = true;
        //アイテムを持たせたのに親を変更してしまうとまずいので省く
        if (hitObj != null && hitObj.ItemSaucer != null && hitObj.ItemSaucer.hasItem)
        {
            changeParent = false;
        }
        Disable(changeParent);

        if(targetMovableObject.name == "Urashima" || targetMovableObject.name == "Otohime")
        {
            ((HoloCharacter)targetMovableObject).ChangeAnimationClip(MotionName.Wait, 0.0f);
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
        if (hit.transform.gameObject.layer == eventAreaLayer) return BaseObjInputHandler.HitObjType.EventArea;

        HoloObject obj = hit.transform.GetComponent<HoloObject>();

        if (obj != null && obj.GetActorType == HoloObject.Type.Character) return BaseObjInputHandler.HitObjType.Character;

        return BaseObjInputHandler.HitObjType.OtherObj;
    }

    /// <summary>
    /// 渡されたオブジェクトに合わせてバウンディングボックスを表示し、操作できるようにする
    /// </summary>
    /// <param name="obj"></param>
    public override void SetTargetObject(HoloObject obj)
    {
        if (!canClick) return;
        if (targetMovableObject != null) targetMovableObject.InputHandler.OnDisabled();
        targetMovableObject = obj;
        if(targetMovableObject.name == "Urashima" || targetMovableObject.name == "Otohime")
        {
            ((HoloCharacter)targetMovableObject).ChangeAnimationClip(MotionName.HangFrom, 0.1f);
        }
        base.SetTargetObject(obj);
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

    public override void Disable(bool setParent = true)
    {
        if (targetMovableObject != null)
        {
            targetMovableObject.InputHandler.OnDisabled();
        }
        base.Disable(setParent);
    }
}
