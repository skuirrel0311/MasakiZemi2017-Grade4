using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestSceneObjController : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    Camera mainCamera;

    [SerializeField]
    LayerMask layer;

    GameObject targetObj;
    NavMeshAgent targetAgent;

    Vector2 oldMousePosition;

    Vector3 zeroVec;

    /// <summary>
    /// つかんだ時に固定する高さ
    /// </summary>
    [SerializeField]
    float operationLockHeight = 2.0f;

    [SerializeField]
    float moveSpeed = 4.0f;

    void Start()
    {
        mainCamera = Camera.main;
        zeroVec = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void Update()
    {
        //右クリック？
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
        
        if(Physics.Raycast(ray, out hit, 10.0f, layer))
        {
            obj = hit.transform.gameObject;

            //つかむことができるかチェック
            Actor actor = obj.GetComponent<Actor>();
            if (actor == null || !actor.isMovable)
            {
                obj = null;
                return false;
            }

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
        if (targetAgent != null) targetAgent.enabled = true;

        targetObj = null;
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
}
