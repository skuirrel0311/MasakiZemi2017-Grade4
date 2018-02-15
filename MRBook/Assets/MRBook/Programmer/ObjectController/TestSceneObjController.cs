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

    float m_draggingTime = 0.0f;

    protected override void Start()
    {
        mainCamera = Camera.main;
        actorManager = ActorManager.I;
        base.Start();
    }

    protected override void Update()
    {
        //右クリック長押し
        if (Input.GetMouseButton(0))
        {
            if (!IsDragging)
            {
                HoloObject obj;
                if (TryGetGameObject(out obj))
                {
                    IsDragging = true;
                    SetTargetObject(obj);
                    StartOperation();
                }
            }
            else
            {
                UpdateOperation();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (IsDragging)
            {
                EndOperation();
                IsDragging = false;
            }
        }
    }

    //短いドラッグ（ドラッグではない）
    void OnTap()
    {
        HoloObject hitObj;
        TryGetGameObject(out hitObj);

    }

    bool TryGetGameObject(out HoloObject holoObject)
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10.0f, layerMask))
        {
            GameObject obj = hit.transform.gameObject;
            //つかむことができるかチェック
            HoloObject actor = obj.GetComponent<HoloObject>();
            if (actor == null || actor.InputHandler == null)
            {
                holoObject = null;
                return false;
            }

            //掴むことができた
            holoObject = actor;
            return true;
        }
        //Debug.Log("don't hit");

        holoObject = null;
        return false;
    }

    protected override void StartOperation()
    {
        base.StartOperation();

        oldMousePosition = Input.mousePosition;
        transform.position = new Vector3(targetMovableObject.transform.position.x, operationLockHeight, targetMovableObject.transform.position.z);
    }

    protected override void UpdateOperation()
    {
        Vector3 velocity = Vector2ComvertToXZVector(GetMouseVelocity());
        Quaternion cam = mainCamera.transform.rotation;
        velocity = Quaternion.Euler(0.0f, cam.eulerAngles.y, 0.0f) * velocity;
        transform.position += velocity * moveSpeed * Time.deltaTime;
        base.UpdateOperation();
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
