using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : BaseManager<ActorManager>
{
    public BasePage currentPage;

    protected override void Start()
    {
        base.Start();
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
}
