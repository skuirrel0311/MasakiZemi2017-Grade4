using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : BaseManager<ActorManager>
{
    MainGameManager gameManager;
    public BasePage currentPage;

    public List<Actor> globalObjectList = new List<Actor>();

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

    //ページが開始された
    void OnPageStarted(BasePage page)
    {
        currentPage = page;

        //前のページから持ってきたオブジェクトがある。
        if(globalObjectList.Count > 0)
        {
            currentPage.actorList.AddRange(globalObjectList);
            globalObjectList.Clear();
        }
    }
}
