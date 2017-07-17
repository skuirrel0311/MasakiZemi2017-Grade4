using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ホログラム全般を管理するクラス
/// </summary>
public class ActorManager : BaseManager<ActorManager>
{
    MainGameManager gameManager;
    public BasePage currentPage;

    public List<Actor> globalActor = new List<Actor>();

    protected override void Start()
    {
        base.Start();
        gameManager = MainGameManager.I;
        gameManager.OnPageChanged += OnPageStarted;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        gameManager.OnPageChanged -= OnPageStarted;
    }

    public Actor GetActor(string name)
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

    public void SetGlobal(Actor actor)
    {
        if (actor == null) return;        

        if(!currentPage.actorList.Remove(actor))
        {
            Debug.LogError(actor.name +  " is not found current page");
        }
        globalActor.Add(actor);
    }

    public List<Actor> GetPageGlobalActor(int currentPageIndex)
    {
        List<Actor> currentPageObjectList = new List<Actor>();

        for(int i = 0;i< globalActor.Count;i++)
        {
            if (globalActor[i].pageIndex != currentPageIndex) continue;

            currentPageObjectList.Add(globalActor[i]);
        }

        return currentPageObjectList;
    }

    //ページが開始された
    void OnPageStarted(BasePage page)
    {
        currentPage = page;

        //前のページから持ってきたオブジェクトがある。
        if(globalActor.Count > 0)
        {
            currentPage.actorList.AddRange(globalActor);
            globalActor.Clear();
        }
    }
}
