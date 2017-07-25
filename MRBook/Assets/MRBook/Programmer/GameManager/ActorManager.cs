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

    protected override void OnDestroy()
    {
        base.OnDestroy();
        gameManager.OnPageChanged -= OnPageChanged;
    }

    public HoloActor GetActor(string name)
    {
        return currentPage.GetActor(name);
    }
    public Transform GetAnchor(string name)
    {
        return currentPage.GetAnchor(name);
    }

    public void DisableActor(string actorName)
    {
        currentPage.DisableActor(actorName);
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
        Debug.Log("changePage in actorManager");
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
