using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ホログラム全般を管理するクラス
/// </summary>
public class ActorManager : BaseManager<ActorManager>
{
    MainGameManager gameManager;
    public BasePage currentPage;

    public List<HoloActor> globalActorList = new List<HoloActor>();

    protected override void Start()
    {
        base.Start();
        gameManager = MainGameManager.I;
        gameManager.OnPageChanged += OnPageChanged;
    }

    /// <summary>
    /// 現在のページに登録されているアクターを返します
    /// </summary>
    public HoloActor GetActor(string name)
    {
        for (int i = 0; i < currentPage.actorList.Count; i++)
        {
            if (currentPage.actorList[i].gameObject.name == name) return currentPage.actorList[i];
        }
        return null;
    }

    /// <summary>
    /// 現在のページに登録されているターゲットポイントを返します
    /// </summary>
    public Transform GetTargetPoint(string name)
    {
        for (int i = 0; i < currentPage.anchorList.Count; i++)
        {
            if (currentPage.anchorList[i].name == name) return currentPage.anchorList[i];
        }

        return null;
    }

    /// <summary>
    /// 現在のページに登録されているアクターを非表示にします
    /// </summary>
    public void DisableActor(string actorName)
    {
        //todo:消す時のエフェクト？
        HoloActor actor = GetActor(actorName);
        if (actor != null) actor.gameObject.SetActive(false);
    }

    public void SetGlobal(HoloActor actor)
    {
        globalActorList.Add(actor);
    }

    public void RemoveGlobal(HoloActor actor)
    {
        if (globalActorList.Remove(actor))
            Debug.Log("remove : " + actor.name);
    }

    public List<HoloActor> GetGlobalActorByPage(int pageIndex)
    {
        List<HoloActor> currentPageObjectList = new List<HoloActor>();

        for(int i = 0;i< globalActorList.Count;i++)
        {
            if (globalActorList[i].pageIndex != pageIndex) continue;

            currentPageObjectList.Add(globalActorList[i]);
        }

        return currentPageObjectList;
    }

    //ページが変更
    void OnPageChanged(BasePage previousPage, BasePage nextPage)
    {
        currentPage = nextPage;

        //前のページから持ってきたオブジェクトがある。
        if(globalActorList.Count > 0)
        {
            //前のページから削除
            for(int i = 0;i < globalActorList.Count;i++)
            {
                previousPage.actorList.Remove(globalActorList[i]);
                //親子関係も変更
                globalActorList[i].transform.parent = nextPage.transform;
            }
        }
    }
}
