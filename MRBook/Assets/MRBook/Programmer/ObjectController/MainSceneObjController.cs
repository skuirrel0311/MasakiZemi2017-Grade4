using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class MainSceneObjController : MyObjControllerByBoundingBox
{
    HoloMovableObject targetActor;
    NavMeshAgent targetAgent;

    //アイテムを掴んでいるか？
    bool isHoldItem = false;

    Ray ray;

    protected override void Start()
    {
        base.Start();

        if (MainSceneManager.I == null) return;
        MainSceneManager.I.OnPlayPage += (page) =>
        {
            Disable();
        };
    }

    protected override void StartDragging()
    {
        if (!canDragging) return;

        if (targetActor.GetActorType == HoloObject.HoloObjectType.Item)
        {
            Debug.Log("is hold item");
            isHoldItem = true;
        }
        targetAgent.enabled = false;

        base.StartDragging();
    }

    protected override void StopDragging()
    {
        Debug.Log("call stop dragging");
        base.StopDragging();
        if (MainSceneManager.I == null) return;

        //直下を調べる
        ray.direction = Vector3.down;
        ray.origin = targetObject.transform.position;

        float radius = 0.0f;
        radius = targetAgent.radius * targetObject.transform.lossyScale.x;

        RaycastHit[] hits = Physics.SphereCastAll(ray, radius, 3.0f);
        HoloMovableObject actor;


        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log("hit " + hits[i].transform.name);
            if (hits[i].transform.tag != "Actor") continue;
            if (hits[i].transform.gameObject.Equals(targetObject)) continue;
            actor = hits[i].transform.GetComponent<HoloMovableObject>();

            if (actor == null) return;
            Debug.Log("hit actor");
            if (actor.GetActorType == HoloObject.HoloObjectType.Character && isHoldItem)
            {
                Debug.Log("call set item");
                isHoldItem = false;
                AkSoundEngine.PostEvent("Equip", gameObject);
                actor.SetItem(targetObject);
            }
            
            return;
        }

        actor = targetObject.GetComponent<HoloMovableObject>();

        //todo:自身の持っているアイテムも含まれる可能性があるので対応
        //自身があるので１ 
        if (hits.Length == 1)
        {
            //ページの外に置いた
            if (actor.isBring) ActorManager.I.SetGlobal(actor);
            //IsBringがtrueじゃない場合でもページ外に留まるという挙動をする
            Debug.Log("hits.Length is 1");
            return;
        }

        //ページ内に配置された

        Debug.Log("in page");
        //キャラクターからアイテムを外した可能性がある
        if (isHoldItem)
        {
            HoloItem item = (HoloItem)targetActor;
            if (item.owner != null)
            {
                item.owner.DumpItem(item.currentHand, false);
                item.owner = null;
            }
        }

        //グローバルに登録されていたら削除する。
        ActorManager.I.RemoveGlobal(actor.name);

        //設定を戻す
        Debug.Log("enable is true");
        targetAgent.enabled = true;
        isHoldItem = false;
    }

    public override void SetTargetObject(GameObject obj)
    {
        base.SetTargetObject(obj);

        if (targetObject != null)
        {
            targetActor = targetObject.GetComponent<HoloMovableObject>();
            targetAgent = targetActor.m_agent;
        }
    }
}
