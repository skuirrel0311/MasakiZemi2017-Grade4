﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : BaseManager<ResultManager>
{
    [SerializeField]
    HoloText totalScore = null;
    [SerializeField]
    HoloButton titleBack = null;

    //玉手箱の中身
    List<HoloItem> secretBoxContentsList = new List<HoloItem>();

    //玉手箱の重さ（重すぎると持ち上げられない?）

    public int deathCount = 0;

    public void AddSecretBoxContent(HoloItem item)
    {
        secretBoxContentsList.Add(item);
    }

    public void RemoveSecretBoxContent(HoloItem item)
    {
        secretBoxContentsList.Remove(item);
    }

    public void RemoveAllSecretBoxContents()
    {
        for(int i = 0;i< secretBoxContentsList.Count;i++)
        {
            secretBoxContentsList[i].owner = null;
        }
        secretBoxContentsList.Clear();
    }
    
    public int GetContentKind()
    {
        if (secretBoxContentsList.Count == 1) return 1;
        return 0;
    }

    public void AddDeathCount()
    {
        deathCount++;

        //todo:上限に達したらゲームオーバー
        const int limit = 99;
        if(deathCount > limit)
        {

        }
    }

    public void ShowTitleBack()
    {
        titleBack.gameObject.SetActive(true);
    }

    public void ShowTotalResult()
    {
        totalScore.CurrentText = "死んだ回数：" + ResultManager.I.deathCount + " 回";

        totalScore.gameObject.SetActive(true);
    }
}
