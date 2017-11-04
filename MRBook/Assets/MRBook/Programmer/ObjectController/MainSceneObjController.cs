using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MainSceneObjController : MyObjControllerByBoundingBox
{
    protected ActorManager actorManager;
    //掴んでいるアクター
    protected HoloMovableObject targetActor;

    //アイテムを掴んでいるか？
    protected bool isHoldItem = false;

    protected Ray ray;
    [SerializeField]
    LayerMask ignoreLayerMask = 1 << 2;
    float sphereCastRadius;

    [SerializeField]
    Transform underTargetMaker = null;
    [SerializeField]
    Transform particle = null;

    //キャッシュ
    Vector3 downVec = Vector3.down;

    const int bookLayer = 11;
    
    protected override void Start()
    {
        base.Start();
        actorManager = ActorManager.I;

        if (MainSceneManager.I == null) return;
        MainSceneManager.I.OnPlayPage += (page) =>
        {
            Disable();
        };
    }

    protected override void StartDragging()
    {
        if (!canDragging) return;

        StartOperation();

        base.StartDragging();
    }

    /// <summary>
    /// 呼ぶ前にtargetActorにActorをセットする
    /// </summary>
    protected virtual void StartOperation()
    {
        if (targetActor.GetActorType == HoloObject.HoloObjectType.Item)
        {
            Debug.Log("is hold item");
            isHoldItem = true;
        }
        targetActor.m_agent.enabled = false;
        sphereCastRadius = targetActor.m_agent.radius * targetActor.transform.lossyScale.x;
    }

    protected override void UpdateDragging()
    {
        base.UpdateDragging();
        UpdateOperation();
    }

    protected virtual void UpdateOperation()
    {
        RaycastHit underObj;

        bool isHitObject = TryGetUnderObject(out underObj);

        particle.gameObject.SetActive(isHitObject);
        underTargetMaker.gameObject.SetActive(isHitObject);

        if (isHitObject)
        {
            Debug.Log("underObj = " + underObj.transform.gameObject.name);
            //ページ内に配置されそう
            particle.position = targetActor.transform.position;
            underTargetMaker.position =underObj.point;
            if (underObj.transform.gameObject.layer == bookLayer)
            {
                //サークルを表示
            }
            else
            {
                //キャラに持たせようとしている可能性もある
                Debug.Log("PresentItem");
            }
        }
    }

    protected override void StopDragging()
    {
        base.StopDragging();
        EndOperation();
    }

    protected virtual void EndOperation()
    {
        particle.gameObject.SetActive(false);
        underTargetMaker.gameObject.SetActive(false);
        //直下を調べる
        ray.direction = Vector3.down;
        ray.origin = targetActor.transform.position;

        float radius = 0.0f;
        radius = targetActor.m_agent.radius * targetActor.transform.lossyScale.x;

        RaycastHit[] hits = Physics.SphereCastAll(ray, radius, 3.0f, ~ignoreLayerMask);
        HoloMovableObject actor;
        int hitObjectNum = hits.Length;

        //下にHoloObjectが有った時の処理
        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log("hit " + hits[i].transform.name);
            if (hits[i].transform.tag != "Actor") continue;
            //こちらでオーバライドしたEqualsを呼び出している(アイテムを持っている時の対応)
            if (targetActor.Equals(hits[i].transform.gameObject)) continue;
            //下にHoloObjectがあった
            actor = hits[i].transform.GetComponent<HoloMovableObject>();

            if (actor == null) return;

            //todo:そのキャラがそのアイテムを持つことが出来るのかどうかの判定をする
            if (actor.GetActorType == HoloObject.HoloObjectType.Character && isHoldItem)
            {
                Debug.Log("call set item");
                isHoldItem = false;
                AkSoundEngine.PostEvent("Equip", gameObject);
                actor.SetItem(targetActor.gameObject);
                //バウンディングボックスは消す
                Disable(false);
            }

            return;
        }

        //下に何もHoloObjectがなかった時の処理

        for (int i = 0; i < hits.Length; i++)
        {
            if (targetActor.Equals(hits[i].transform.gameObject))
            {
                //自身以外の数が知りたいので減らす
                hitObjectNum--;
                continue;
            }
        }

        //ページの外に置いた
        if (hitObjectNum == 0)
        {
            if (targetActor.isBring) actorManager.SetGlobal(targetActor);
            //IsBringがtrueじゃない場合でもページ外に留まるという挙動をする
            Debug.Log(targetActor.name + "is out book");
            return;
        }

        //下にHoloObject以外がある(ページ内に配置された)
        Debug.Log(targetActor + " is put page");

        //キャラクターからアイテムを外した可能性がある
        if (isHoldItem)
        {
            HoloItem item = (HoloItem)targetActor;

            //誰かに所有されていたら
            if (item.owner != null)
            {
                Debug.Log("call dump item");
                item.owner.DumpItem(item.currentHand, false);
            }
        }

        //グローバルに登録されていたら削除する。
        actorManager.RemoveGlobal(targetActor.name);

        //設定を戻す
        isHoldItem = false;
        //バウンディングボックスは消す
        Disable();
        //床に落とす
        if(!targetActor.isFloating) StartCoroutine(CheckUnderNavMesh());
    }

    //渡されたオブジェクトに合わせてバウンディングボックスを表示し、操作できるようにする
    public override void SetTargetObject(GameObject obj)
    {
        base.SetTargetObject(obj);

        if (targetObject != null)
        {
            targetActor = targetObject.GetComponent<HoloMovableObject>();
        }
    }

    //直下に本のメッシュはあるが、遠すぎて反応できていなかった場合のみ使用する
    IEnumerator CheckUnderNavMesh()
    {
#if UNITY_EDITOR
        MainSceneManager mainSceneManager = TestSceneManager.I;
#else
        MainSceneManager mainSceneManager = MainSceneManager.I;
#endif

        NavMeshHit hit;
        while (true)
        {
            if (targetActor == null || targetActor.m_agent == null)
            {
                Debug.Log("don't nav checked");
                break;
            }
            //0.1ずつ下を探す
            targetActor.transform.position += Vector3.down * 0.1f;

            //todo : 絵本よりも下に行った場合はやばいのでなにか対応が必要

            if (NavMesh.SamplePosition(targetActor.transform.position, out hit, targetActor.m_agent.height, NavMesh.AllAreas))
            {
                Debug.Log("nav checked");
                break;
            }

            yield return null;
        }

        Debug.Log("end nav checked");
        if (!targetActor.isFloating)
        {
            targetActor.m_agent.enabled = true;
        }
        targetActor = null;
    }
    
    /// <summary>
    /// 掴んでいるオブジェクトの直下のオブジェクトを取得する
    /// </summary>
    bool TryGetUnderObject(out RaycastHit hitObj)
    {
        //直下を調べる
        ray.direction = Vector3.down;
        ray.origin = targetActor.transform.position;
        float maxDistance = 3.0f;
        float currentDistance = maxDistance;
        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereCastRadius, maxDistance, ~ignoreLayerMask);
        hitObj = new RaycastHit();
        bool isHit = false;

        for (int i = 0; i < hits.Length; i++)
        {
            //こちらでオーバライドしたEqualsを呼び出している(アイテムを持っている時の対応)
            if (targetActor.Equals(hits[i].transform.gameObject)) continue;

            isHit = true;
            //自身以外に当たった
            if (hits[i].distance < currentDistance)
            {
                hitObj = hits[i];
            }
        }

        return isHit;
    }
}
