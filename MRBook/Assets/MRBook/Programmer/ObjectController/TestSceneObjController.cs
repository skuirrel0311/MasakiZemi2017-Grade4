using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestSceneObjController : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    Camera mainCamera;
    ActorManager actorManager;

    //Actor
    LayerMask layerMask = 1 << 8;

    public GameObject targetObj;
    NavMeshAgent targetAgent;

    Vector2 oldMousePosition;

    //計算用キャッシュ
    Vector3 zeroVec;

    /// <summary>
    /// つかんだ時に固定する高さ
    /// </summary>
    [SerializeField]
    float operationLockHeight = 2.0f;

    [SerializeField]
    float moveSpeed = 4.0f;

    //アイテムを掴んでいるか？
    public bool isHoldItem = false;

    void Start()
    {
        mainCamera = Camera.main;
        actorManager = ActorManager.I;

        zeroVec = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void Update()
    {
        //右クリック
        if(Input.GetMouseButtonDown(0))
        {
            if (TryGetGameObject(out targetObj))
            {
                StartOperationObject();
            }
        }

        //右クリック長押し
        if (Input.GetMouseButton(0))
        {
            if (targetObj != null) UpdateObjectOperation();

        }

        if(Input.GetMouseButtonUp(0))
        {
            EndOperationObject();
        }
    }

    bool TryGetGameObject(out GameObject obj)
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out hit, 10.0f, layerMask))
        {
            obj = hit.transform.gameObject;

            //つかむことができるかチェック
            HoloMovableObject actor = obj.GetComponent<HoloMovableObject>();
            if (actor == null || !actor.isMovable)
            {
                obj = null;
                return false;
            }

            if (actor.GetActorType == HoloObject.HoloObjectType.Item) isHoldItem = true;
            return true;
        }
        else
        {
            obj = null;
        }

        return false;
    }

    void StartOperationObject()
    {
        //つかむ動作
        targetAgent = targetObj.GetComponent<NavMeshAgent>();
        if (targetAgent != null) targetAgent.enabled = false;
        oldMousePosition = Input.mousePosition;
        targetObj.transform.position = new Vector3(targetObj.transform.position.x, operationLockHeight, targetObj.transform.position.z);
    }

    void UpdateObjectOperation()
    {
        Vector3 velocity = Vector2ComvertToXZVector(GetMouseVelocity());
        Quaternion cam = mainCamera.transform.rotation;
        velocity = Quaternion.Euler(0.0f, cam.eulerAngles.y, 0.0f) * velocity;
        targetObj.transform.position += velocity * moveSpeed * Time.deltaTime;
    }

    void EndOperationObject()
    {
        if (targetObj == null) return;

        //離した時に直下に何かオブジェクトがあったら困るので対応
        ray.direction = Vector3.down;
        ray.origin = targetObj.transform.position;
        float radius = targetAgent.radius * targetObj.transform.lossyScale.x;
        HoloMovableObject actor;
        
        RaycastHit[] hits = Physics.SphereCastAll(ray, radius, 2.0f);
        for (int i = 0;i< hits.Length;i++)
        {
            Debug.Log("hitobj = " + hits[i].transform.name);
            if (hits[i].transform.gameObject.Equals(targetObj)) continue;
            if (hits[i].transform.tag != "Actor") continue;
            //なんか当たった
            actor = hits[i].transform.GetComponent<HoloMovableObject>();

            if (actor == null) return;
            if(actor.GetActorType == HoloObject.HoloObjectType.Character && isHoldItem)
            {
                Debug.Log("call set item");
                AkSoundEngine.PostEvent("Equip", gameObject);
                isHoldItem = false;
                actor.SetItem(targetObj);
            }

            return;
        }

        actor = targetObj.GetComponent<HoloMovableObject>();


        if (hits.Length == 1)
        {
            //ページの外に置いた
            if(actor.isBring) actorManager.SetGlobal(actor);
            //IsBringがtrueじゃない場合でもページ外に留まるという挙動をする
            return;
        }
        
        //ページ内に配置された

        //キャラクターからアイテムを外した可能性がある
        if(isHoldItem)
        {
            HoloItem item = targetObj.GetComponent<HoloItem>();
            if (item.owner != null)
            {
                item.owner.DumpItem(item.currentHand, false);
                item.owner = null;
            }
        }

        //グローバルに登録されていたら削除する。
        actorManager.RemoveGlobal(actor.name);

        //NavMeshAgentを戻す
        isHoldItem = false;

        StartCoroutine(CheckUnderNavMesh());
    }

    /// <summary>
    /// 正規化したマウスの移動量を返す
    /// </summary>
    Vector2 GetMouseVelocity()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 velocity = mousePosition - oldMousePosition;
        
        oldMousePosition = mousePosition;
        return velocity.normalized;
    }

    Vector3 Vector2ComvertToXZVector(Vector2 vec)
    {
        Vector3 temp = zeroVec;

        temp.x = vec.x;
        temp.z = vec.y;             

        return temp;
    }

    //直下に本のメッシュはあるが、遠すぎて反応できていなかった場合のみ使用する
    IEnumerator CheckUnderNavMesh()
    {
        NavMeshHit hit;
        while(true)
        {
            if (targetAgent == null) break;
            //0.1ずつ下を探す
            targetObj.transform.position += Vector3.down * 0.1f;

            if(NavMesh.SamplePosition(targetAgent.transform.position, out hit, targetAgent.height * 2, NavMesh.AllAreas))
            {
                break;
            }

            yield return null;
        }

        targetAgent.enabled = true;
        targetAgent = null;
        targetObj = null;
    }
}
