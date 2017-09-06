using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class TitleView : HoloMovableObject
{
    [SerializeField]
    HoloButton tapToStart = null;

    [SerializeField]
    GameObject rootObj = null;

    MyGameManager gameManager;
    MyObjControllerByBoundingBox objController;

    void Start()
    {
        gameManager = MyGameManager.I;
        objController = MyObjControllerByBoundingBox.I;
    }

    public override void OnInputClicked(InputClickedEventData eventData)
    {
        base.OnInputClicked(eventData);
        gameManager.SetWorldAnchorsRendererActive(!objController.canDragging);

        SetButtonActive(!objController.canDragging);
    }

    public void AllHide()
    {
        BoxCollider[] cols = GetComponents<BoxCollider>();
        for(int i = 0;i< cols.Length;i++)
        {
            cols[i].enabled = false;
        }
        rootObj.SetActive(false);
    }

    public void SetButtonActive(bool isActive)
    {
        if (isActive)
            tapToStart.Refresh();
        else
            tapToStart.Hide();
    }
}
