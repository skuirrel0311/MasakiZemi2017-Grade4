using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class TitleView : HoloMovableObject
{
    [SerializeField]
    HoloButton tapToStart = null;

    [SerializeField]
    GameObject rootObj = null;
    
    MyObjControllerByBoundingBox objController;

    void Start()
    {
        objController = MyObjControllerByBoundingBox.I;
    }

    public override void OnInputClicked(InputClickedEventData eventData)
    {
        base.OnInputClicked(eventData);
        SetButtonActive(!objController.canDragging);
    }

    public void HideAll()
    {
        BoxCollider[] cols = GetComponents<BoxCollider>();
        for(int i = 0;i< cols.Length;i++)
        {
            cols[i].enabled = false;
        }
        rootObj.SetActive(false);

        SetButtonActive(false);
        BookPositionModifier.I.SetWorldAnchorsRendererActive(false);
    }

    void SetButtonActive(bool isActive)
    {
        if (isActive)
            tapToStart.Refresh();
        else
            tapToStart.Hide();
    }
}
