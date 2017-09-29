﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasePage : MonoBehaviour
{
    /// <summary>
    /// お題
    /// </summary>
    public string missionText = "";

    int pageIndex = 0;

    public RuntimeAnimatorController controller = null;

    /// <summary>
    /// オクルージョン用のオブジェクト
    /// </summary>
    [SerializeField]
    GameObject[] bookObjects = null;


    //ページに存在するアンカー(何かを発生させる位置のこと)のリスト
    public Dictionary<string, Transform> targetPointDictionary = new Dictionary<string, Transform>();

    //同名のオブジェクトがあるときには使えない
    public Dictionary<string, HoloObject> objectDictionary = new Dictionary<string, HoloObject>();
    //ページに存在するホログラム全部。(灰色にさせたりするときに必要)
    public List<HoloObject> objectList = new List<HoloObject>();
    //動かせるホログラム全部
    public Dictionary<string, HoloMovableObject> movableObjectDictionary = new Dictionary<string, HoloMovableObject>();
    //よく使うのでリストでも管理
    public List<HoloMovableObject> movableObjectList = new List<HoloMovableObject>();

    /// <summary>
    /// 本の位置にページを固定する
    /// </summary>
    public void PageLock(Vector3 position, Quaternion rotation, int pageIndex)
    {
        transform.SetPositionAndRotation(position, rotation);
        for (int i = 0; i < movableObjectList.Count; i++)
        {
            movableObjectList[i].ApplyDefaultTransform();
        }
        this.pageIndex = pageIndex;
    }

    /// <summary>
    /// ページを開いた時に呼ぶ
    /// </summary>
    public void PageStart(bool isFirst)
    {
        Debug.Log("is first = " + isFirst + " page index = " + pageIndex);
        if (!isFirst)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].PageStart(pageIndex, true);
            }
            return;
        }

        //初回のみリストに格納する
        GameObject[] tempArray;

        tempArray = GameObject.FindGameObjectsWithTag("Actor");
        for (int i = 0; i < tempArray.Length; i++)
        {
            HoloObject actor = tempArray[i].GetComponent<HoloObject>();
            if (actor == null)
            {
                Debug.Log(tempArray[i].name + "is not actor");
                continue;
            }

            Debug.Log("add actor " + actor.name);
            objectList.Add(actor);

            try
            {
                objectDictionary.Add(actor.name, actor);
            }
            catch
            {
                //同名のオブジェクトを追加しようとした
                continue;
            }
            if (actor.GetActorType != HoloObject.HoloObjectType.Statics)
            {
                movableObjectList.Add((HoloMovableObject)actor);
                movableObjectDictionary.Add(actor.name, (HoloMovableObject)actor);
            }
        }

        tempArray = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < tempArray.Length; i++)
        {
            Debug.Log("add target point " + tempArray[i].name);
            targetPointDictionary.Add(tempArray[i].name, tempArray[i].transform);
        }
        Material visibleMat = MainSceneManager.I.visibleMat;
        for (int i = 0; i < bookObjects.Length; i++)
        {
            bookObjects[i].GetComponent<Renderer>().material = visibleMat;
        }

        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].PageStart(pageIndex, true);
        }
    }

    public void PlayPage()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].ResetShader();
        }
    }

    /// <summary>
    /// このページに登録されているアクターをページを開いた時の位置に戻す
    /// </summary>
    public void ResetPage()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].ResetTransform();
        }
        //todo:リセット中のアニメーション
    }

    public void SetAllAgentEnabled(bool enabled)
    {
        for (int i = 0; i < movableObjectList.Count; i++)
        {
            movableObjectList[i].m_agent.enabled = enabled;
        }
    }

    public void AddMovableObject(HoloMovableObject obj)
    {
        movableObjectList.Add(obj);
        movableObjectDictionary.Add(obj.name, obj);
    }

    public void RemoveMovableObject(string name)
    {
        movableObjectList.Remove(movableObjectDictionary[name]);
        movableObjectDictionary.Remove(name);
    }
}
