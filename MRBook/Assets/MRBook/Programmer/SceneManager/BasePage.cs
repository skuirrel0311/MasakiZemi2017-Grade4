﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //ページに存在するアクター(ホログラムのオブジェクトのこと)のリスト
    [System.NonSerialized]
    public List<HoloActor> actorList = new List<HoloActor>();
    //ページに存在するアンカー(何かを発生させる位置のこと)のリスト
    [System.NonSerialized]
    public List<Transform> anchorList = new List<Transform>();

    //このページを開くのが初めてか？
    bool isFirst = true;

    /// <summary>
    /// 本の位置にページを固定する
    /// </summary>
    public void PageLock(Vector3 position, Quaternion rotation, int pageIndex)
    {
        transform.position = position;
        transform.rotation = rotation;
        this.pageIndex = pageIndex;
    }

    /// <summary>
    /// ページを開いた時に呼ぶ
    /// </summary>
    public void PageStart(bool isBack)
    {
        if (!isFirst)
        {
            for (int i = 0; i < actorList.Count; i++)
            {
                actorList[i].PageStart(pageIndex, false);
            }
            return;
        }

        isFirst = false;
        //初回のみリストに格納する
        GameObject[] tempArray;

        //Actorとはホログラム全般のこと（キャラクター、小物など。背景は含まない）
        tempArray = GameObject.FindGameObjectsWithTag("Actor");
        for (int i = 0; i < tempArray.Length; i++)
        {
            HoloActor actor = tempArray[i].GetComponent<HoloActor>();
            if(actor != null) actorList.Add(actor);
        }
        //Targetとは目印のこと
        //todo:StaticTargetはベイクできるのでそれから取得
        tempArray = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < tempArray.Length; i++)
        {
            anchorList.Add(tempArray[i].transform);
        }

        for (int i = 0; i < actorList.Count; i++)
        {
            actorList[i].PageStart(pageIndex, true);
        }

        Material visibleMat = MainGameManager.I.visibleMat;
        for (int i = 0; i < bookObjects.Length; i++)
        {
            bookObjects[i].GetComponent<Renderer>().material = visibleMat;
        }
    }

    /// <summary>
    /// このページに登録されているアクターをページを開いた時の位置に戻す
    /// </summary>
    /// <param name="endCallBack"></param>
    public void ResetPage()
    {
        for (int i = 0; i < actorList.Count; i++)
        {
            actorList[i].ResetTransform();
        }

        //todo:リセット中のアニメーション
    }
}
