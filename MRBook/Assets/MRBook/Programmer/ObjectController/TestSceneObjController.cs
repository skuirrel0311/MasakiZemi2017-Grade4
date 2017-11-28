﻿using System.Collections;
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
        actorManager = ActorManager.I;
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
            if (targetMovableObject != null) UpdateOperation();

        }

        if(Input.GetMouseButtonUp(0))
        {
            if(targetMovableObject != null) EndOperation();
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
            HoloObject actor = obj.GetComponent<HoloObject>();
            if (actor == null || actor.InputHandler == null)
            {
                targetMovableObject = null;
                return false;
            }

            //掴むことができた
            targetMovableObject = actor;
            return true;
        }
        //Debug.Log("don't hit");

        targetMovableObject = null;
        return false;
    }
    
    protected override void StartOperation()
    {
        base.StartOperation();

        oldMousePosition = Input.mousePosition;
        targetMovableObject.transform.position = new Vector3(targetMovableObject.transform.position.x, operationLockHeight, targetMovableObject.transform.position.z);
    }

    protected override void UpdateOperation()
    {
        Vector3 velocity = Vector2ComvertToXZVector(GetMouseVelocity());
        Quaternion cam = mainCamera.transform.rotation;
        velocity = Quaternion.Euler(0.0f, cam.eulerAngles.y, 0.0f) * velocity;
        targetMovableObject.transform.position += velocity * moveSpeed * Time.deltaTime;
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
