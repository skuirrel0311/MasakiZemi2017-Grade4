using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestSceneObjController : MainSceneObjController
{
    Vector2 oldMousePosition;

    /// <summary>
    /// つかんだ時に固定する高さ
    /// </summary>
    [SerializeField]
    float operationLockHeight = 2.0f;

    [SerializeField]
    float moveSpeed = 4.0f;

    //ベースを呼ばないために宣言する
    protected override void Start()
    {
        mainCamera = Camera.main;
    }

    protected override void Update()
    {
        //右クリック
        if(Input.GetMouseButtonDown(0))
        {
            if (TryGetGameObject())
            {
                StartOperation();
            }
        }

        //右クリック長押し
        if (Input.GetMouseButton(0))
        {
            if (targetActor != null) UpdateOperation();

        }

        if(Input.GetMouseButtonUp(0))
        {
            if(targetActor != null) EndOperation();
        }
    }

    bool TryGetGameObject()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if(Physics.Raycast(ray, out hit, 10.0f, layerMask))
        {
            GameObject obj = hit.transform.gameObject;

            //つかむことができるかチェック
            HoloMovableObject actor = obj.GetComponent<HoloMovableObject>();
            if (actor == null || !actor.isMovable)
            {
                targetActor = null;
                return false;
            }
            
            //掴むことができた
            targetActor = actor;
            return true;
        }

        targetActor = null;
        return false;
    }
    
    protected override void StartOperation()
    {
        base.StartOperation();

        oldMousePosition = Input.mousePosition;
        targetActor.transform.position = new Vector3(targetActor.transform.position.x, operationLockHeight, targetActor.transform.position.z);
    }

    void UpdateOperation()
    {
        Vector3 velocity = Vector2ComvertToXZVector(GetMouseVelocity());
        Quaternion cam = mainCamera.transform.rotation;
        velocity = Quaternion.Euler(0.0f, cam.eulerAngles.y, 0.0f) * velocity;
        targetActor.transform.position += velocity * moveSpeed * Time.deltaTime;
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
        Vector3 temp;

        temp.x = vec.x;
        temp.y = 0.0f;
        temp.z = vec.y;             

        return temp;
    }
}
