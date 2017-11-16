﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MainSceneObjController : MyObjControllerByBoundingBox
{
    protected ActorManager actorManager;
    //掴んでいるアクター
    protected HoloMovableObject targetObj;

    //アイテムを掴んでいるか？
    protected bool isHoldItem = false;

    protected Ray ray;
    [SerializeField]
    LayerMask ignoreLayerMask = 1 << 2;

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
        if (targetObj.IsGrounding)
        {
            ((HoloGroundingObject)targetObj).m_agent.enabled = false;
        }

        if (targetObj.GetActorType == HoloObject.Type.Item)
        {
            isHoldItem = true;
            //todo:ここでアイテムの所有権を放棄しなければならない
        }
    }

    protected virtual void UpdateOperation()
    {
        RaycastHit underObj;

        bool isHitObject = TryGetUnderObject(out underObj);

        particle.gameObject.SetActive(isHitObject);
        underTargetMaker.gameObject.SetActive(isHitObject);

        if (!isHitObject) return;

        Debug.Log("underObj = " + underObj.transform.gameObject.name);
        //ページ内に配置されそう
        particle.position = targetObj.transform.position;
        underTargetMaker.position = underObj.point;
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

    protected virtual void EndOperation()
    {
        particle.gameObject.SetActive(false);
        underTargetMaker.gameObject.SetActive(false);

        //地面に落とす必要があるか？
        if (!targetObj.IsGrounding) return;

        HoloGroundingObject groundingObj = (HoloGroundingObject)targetObj;

        //直下を調べる
        ray.direction = Vector3.down;
        ray.origin = groundingObj.transform.position;
        float radius = groundingObj.SphereCastRadius;

        RaycastHit[] hits = Physics.SphereCastAll(ray, radius, 3.0f, ~ignoreLayerMask);
        HoloMovableObject hitActor;
        int hitObjectNum = hits.Length;

        //下にHoloObjectが有った時の処理
        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log("hit " + hits[i].transform.name);
            if (hits[i].transform.tag != "Actor") continue;
            //こちらでオーバライドしたEqualsを呼び出している(アイテムを持っている時の対応)
            if (targetObj.Equals(hits[i].transform.gameObject)) continue;
            //下にHoloObjectがあった
            hitActor = hits[i].transform.GetComponent<HoloMovableObject>();

            if (hitActor == null) return;

            //todo:そのキャラがそのアイテムを持つことが出来るのかどうかの判定をする
            if (hitActor.GetActorType == HoloObject.Type.Character && isHoldItem)
            {
                Debug.Log("call set item");
                isHoldItem = false;
                AkSoundEngine.PostEvent("Equip", gameObject);
                hitActor.SetItem(targetObj.gameObject);
                //バウンディングボックスは消す
                Disable(false);
            }

            return;
        }

        //下に何もHoloObjectがなかった時の処理

        for (int i = 0; i < hits.Length; i++)
        {
            if (targetObj.Equals(hits[i].transform.gameObject))
            {
                //自身以外の数が知りたいので減らす
                hitObjectNum--;
                continue;
            }
        }

        //ページの外に置いた
        if (hitObjectNum == 0)
        {
            if (targetObj.isBring) actorManager.SetGlobal(targetObj);
            //IsBringがtrueじゃない場合でもページ外に留まるという挙動をする
            Debug.Log(targetObj.name + "is out book");
            return;
        }

        //下にHoloObject以外がある(ページ内に配置された)
        Debug.Log(targetObj + " is put page");

        //キャラクターからアイテムを外した可能性がある
        if (isHoldItem)
        {
            HoloItem item = (HoloItem)targetObj;

            //誰かに所有されていたら
            if (item.owner != null)
            {
                //todo:ここで所有権を破棄するのではなく、
                Debug.Log("call dump item");
                item.owner.DumpItem(item.currentHand, false);
            }
        }

        //グローバルに登録されていたら削除する。
        actorManager.RemoveGlobal(targetObj.name);

        //設定を戻す
        isHoldItem = false;
        //バウンディングボックスは消す
        Disable();
        //床に落とす
        targetObj.Fall();
    }

    /// <summary>
    /// 渡されたオブジェクトに合わせてバウンディングボックスを表示し、操作できるようにする
    /// </summary>
    /// <param name="obj"></param>
    public override void SetTargetObject(GameObject obj)
    {
        base.SetTargetObject(obj);

        targetObj = obj.GetComponent<HoloMovableObject>();
    }

    /// <summary>
    /// 掴んでいるオブジェクトの直下のオブジェクトを取得する
    /// </summary>
    bool TryGetUnderObject(out RaycastHit hitObj)
    {
        //直下を調べる
        ray.direction = Vector3.down;
        ray.origin = targetObj.transform.position;
        const float maxDistance = 3.0f;
        RaycastHit[] hits = Physics.SphereCastAll(ray, targetObj.SphereCastRadius, maxDistance, ~ignoreLayerMask);
        hitObj = new RaycastHit();
        bool isHit = false;

        for (int i = 0; i < hits.Length; i++)
        {
            //こちらでオーバライドしたEqualsを呼び出している(アイテムを持っている時の対応)
            if (targetObj.Equals(hits[i].transform.gameObject)) continue;

            isHit = true;
            //自身以外に当たった
            if (hits[i].distance < maxDistance)
            {
                hitObj = hits[i];
            }
        }

        return isHit;
    }
}
