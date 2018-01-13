using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParent : BaseStateMachineBehaviour
{
    public enum ParentType { HoloObject, Page }

    [SerializeField]
    ParentType parentType = ParentType.HoloObject;

    [SerializeField]
    string targetName = "";

    [SerializeField]
    string parentName = "";

    protected override void OnStart()
    {
        base.OnStart();

        HoloObject obj = ActorManager.I.GetObject(targetName);

        if(obj == null)
        {
            Debug.LogError("not found " + targetName);
            return;
        }
        obj.transform.parent = GetParent(parentType);
    }

    Transform GetParent(ParentType type)
    {
        BasePage currentPage = MainSceneManager.I.pages[MainSceneManager.I.currentPageIndex];

        switch(type)
        {
            case ParentType.Page:
                return currentPage.transform;
            case ParentType.HoloObject:
                HoloObject obj = ActorManager.I.GetObject(parentName);
                if (obj == null) return null;
                return obj.transform;
        }
        return null;
    }
}
