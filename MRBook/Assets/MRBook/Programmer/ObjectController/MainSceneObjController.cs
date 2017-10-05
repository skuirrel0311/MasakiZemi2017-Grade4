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
    /// 呼ぶ前にtargetActorにActorがセットする
    /// </summary>
    protected virtual void StartOperation()
    {
        if (targetActor.GetActorType == HoloObject.HoloObjectType.Item)
        {
            Debug.Log("is hold item");
            isHoldItem = true;
        }
        targetActor.m_agent.enabled = false;
    }

    protected override void StopDragging()
    {
        base.StopDragging();
        EndOperation();
    }

    protected virtual void EndOperation()
    {
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
            if (targetActor.Equals(hits[i].transform.gameObject))continue;

            //下にHoloObjectがあった
            actor = hits[i].transform.GetComponent<HoloMovableObject>();

            if (actor == null) return;

            //todo:そのキャラがそのアイテムを持つことが出来るのかどうかの判定をする
            if (actor.GetActorType == HoloObject.HoloObjectType.Character && isHoldItem)
            {
                Debug.LogWarning("call set item");
                isHoldItem = false;
                AkSoundEngine.PostEvent("Equip", gameObject);
                actor.SetItem(targetObject);
//#if !UNITY_EDITOR
                //バウンディングボックスは消す
                Disable(false);
//#endif
            }

            return;
        }

        //下に何もHoloObjectがなかった時の処理

        for(int i = 0;i< hits.Length;i++)
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
        Debug.Log(targetActor +  " is put page");

        //キャラクターからアイテムを外した可能性がある
        if (isHoldItem)
        {
            HoloItem item = (HoloItem)targetActor;

            //誰かに所有されていたら
            if (item.owner != null)
            {
                item.owner.DumpItem(item.currentHand, false);
                item.owner = null;
            }
        }

        //グローバルに登録されていたら削除する。
        actorManager.RemoveGlobal(targetActor.name);

        //設定を戻す
        targetActor.m_agent.enabled = true;
        isHoldItem = false;
        //バウンディングボックスは消す
//#if !UNITY_EDITOR
        Disable();
//#endif
        StartCoroutine(CheckUnderNavMesh());
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
        MainSceneManager mainSceneManager = TestSceneManager.I;

        NavMeshHit hit;
        while (true)
        {
            if (targetActor == null || targetActor.m_agent) break;
            //0.1ずつ下を探す
            targetActor.transform.position += Vector3.down * 0.1f;

            if (NavMesh.SamplePosition(targetActor.m_agent.transform.position, out hit, targetActor.m_agent.height * 2, NavMesh.AllAreas))
            {
                break;
            }

            yield return null;
        }

        //todo:エフェクト（煙？）
        targetActor.m_agent.enabled = true;
        targetActor = null;
    }
}
