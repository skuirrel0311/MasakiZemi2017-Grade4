using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class TitleView : HoloMovableObject
{
    [SerializeField]
    HoloButton tapToStart = null;

    [SerializeField]
    GameObject rootObj = null;

    [SerializeField]
    GameObject toFollowObjContainer = null;

    BookPositionModifier modifier;
    MyObjControllerByBoundingBox objController;

    bool isHide = false;

    void Start()
    {
        modifier = BookPositionModifier.I;
        objController = MyObjControllerByBoundingBox.I;
    }

    void Update()
    {
        if (isHide) return;
        toFollowObjContainer.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    public override void OnInputClicked(InputClickedEventData eventData)
    {
        base.OnInputClicked(eventData);

        modifier.WorldAnchorsOperation(!objController.canDragging);
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
        //gameManager.SetWorldAnchorsRendererActive(false);

        isHide = true;
    }

    public void SetButtonActive(bool isActive)
    {
        if (isActive)
            tapToStart.Refresh();
        else
            tapToStart.Hide();
    }
}
