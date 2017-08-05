using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class MainSceneObjController : MyObjPositionController
{
    bool canDragging = false;
    Renderer[] m_renderers;
    Transform oldParent = null;

    Ray ray;
    RaycastHit hit;
    HoloActor targetActor;
    NavMeshAgent targetAgent;
    Vector3 offset;
    //Actor
    LayerMask layerMask = 1 << 8;

    //アイテムを掴んでいるか？
    bool isHoldItem = false;
    
    static MainSceneObjController instance;
    public static MainSceneObjController I
    {
        get
        {
            if (instance == null)
            {
                instance = (MainSceneObjController)FindObjectOfType(typeof(MainSceneObjController));
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
        List<MainSceneObjController> instances = new List<MainSceneObjController>();
        instances.AddRange((MainSceneObjController[])FindObjectsOfType(typeof(MainSceneObjController)));

        if (I == null) I = instances[0];
        instances.Remove(I);

        if (instances.Count == 0) return;
        //あぶれ者のinstanceはデストロイ 
        foreach (MainSceneObjController t in instances) Destroy(t.gameObject);
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= WasLoaded;
        I = null;
    }

    protected override void Start()
    {
        m_renderers = GetComponentsInChildren<Renderer>();

        base.Start();

        if (MainGameManager.I == null) return;
        MainGameManager.I.OnPlayPage += (page) =>
        {
            Disable();
        };


    }

    protected override void Update()
    {
        base.Update();
        
        if(canDragging && !isDragging)
        {
            Vector3 actorPosition = targetObject.transform.position;
            transform.position = actorPosition + offset;

            targetObject.transform.position = actorPosition;
        }
    }

    protected override void StartDragging()
    {
        if (!canDragging) return;

        if (targetActor.GetActorType == HoloActor.ActorType.Item) isHoldItem = true;
        targetAgent.enabled = false;

        base.StartDragging();
    }

    protected override void StopDragging()
    {
        base.StopDragging();
        if (MainGameManager.I == null) return;

        //直下を調べる
        ray.direction = Vector3.down;
        ray.origin = targetObject.transform.position;
        
        float radius = 0.0f;
        radius = targetAgent.radius * targetObject.transform.lossyScale.x;

        RaycastHit[] hits = Physics.SphereCastAll(ray, radius, targetObject.transform.position.y + 0.5f);
        HoloActor actor;


        for(int i = 0; i < hits.Length;i++)
        {
            if (hits[i].transform.tag != "Actor") continue;
            if (hits[i].transform.gameObject.Equals(targetObject)) continue;
            actor = hits[i].transform.GetComponent<HoloActor>();
            if(actor.GetActorType == HoloActor.ActorType.Character && isHoldItem)
            {
                actor.SetItem(targetObject);
            }
            return;
        }

        actor = targetObject.GetComponent<HoloActor>();

        //自身があるので１ todo:自身の持っているアイテムも含まれる可能性があるので対応
        if (hits.Length == 1)
        {
            //ページの外に置いた
            if (actor.isBring) ActorManager.I.SetGlobal(actor);
            //IsBringがtrueじゃない場合でもページ外に留まるという挙動をする
            return;
        }

        //ページ内に配置された

        //キャラクターからアイテムを外した可能性がある
        if (isHoldItem)
        {
            HoloItem item = (HoloItem)targetActor;
            if (item.owner != null) item.owner.DumpItem(item.currentHand, false);
        }

        //グローバルに登録されていたら削除する。
        ActorManager.I.RemoveGlobal(actor);

        //設定を戻す
        targetAgent.enabled = true;
        isHoldItem = false;
    }

    public void ChangeWireFrameView(bool canDragging)
    {
        this.canDragging = canDragging;

        for (int i = 0; i < m_renderers.Length; i++)
        {
            m_renderers[i].enabled = canDragging;
        }
    }

    //中断
    public void Disable()
    {
        if (targetObject != null)
        {
            targetObject.transform.parent = oldParent;
        }
        targetObject = null;

        ChangeWireFrameView(false);
    }

    public void SetTargetObject(GameObject obj)
    {
        if (targetObject != null && targetObject.Equals(obj))
        {
            Disable();
            return;
        }

        if (targetObject != null)
        {
            targetObject.transform.parent = oldParent;
        }
        //targetObjectの切り替え
        targetObject = obj;
        targetAgent = targetObject.GetComponent<NavMeshAgent>();
        targetActor = targetObject.GetComponent<HoloActor>();

        transform.position = targetObject.transform.position;

        BoxCollider boxCol = targetObject.GetComponent<BoxCollider>();

        Vector3 boxSize = targetObject.transform.lossyScale;
        boxSize.x *= boxCol.size.x;
        boxSize.y *= boxCol.size.y;
        boxSize.z *= boxCol.size.z;

        transform.localScale = boxSize;

        offset = targetObject.transform.lossyScale;
        offset.x *= boxCol.center.x;
        offset.y *= boxCol.center.y;
        offset.z *= boxCol.center.z;

        transform.position += offset;
        transform.rotation = targetObject.transform.rotation;

        ChangeWireFrameView(true);

        oldParent = obj.transform.parent;
        obj.transform.parent = transform;
    }
}
