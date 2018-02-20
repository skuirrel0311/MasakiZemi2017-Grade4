using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class MainSceneCursor : MyCursor
{
    [SerializeField]
    int actorLayer = 8;

    HoloObjectController objController;

    public static MainSceneCursor I;

    void Awake()
    {
        if(I == null)
        {
            I = this;
        }
        else
        {
            Destroy(this);
        }
    }

    protected override void Start()
    {
        base.Start();
        objController = HoloObjectController.I;
    }

    protected override void OnFocusedObjectChanged(GameObject previousObject, GameObject newObject)
    {
        base.OnFocusedObjectChanged(previousObject, newObject);

        //ホールド中
        if (isRecognizedPosCon && isRecognizedHold) return;

        //普通のカーソル
        if(newObject == null || newObject.layer != actorLayer)
        {
            isRecognizedPosCon = false;
            isRecognizedRotCon = false;
            isRecognizedHold = false;
            OnFlagChanged();
            return;
        }

        if (objController != null && !objController.canClick) return;

        //矢印付きのカーソル
        isRecognizedPosCon = true;
        OnFlagChanged();
    }

    public void Refresh()
    {
        isRecognizedPosCon = false;
        isRecognizedRotCon = false;
        isRecognizedHold = false;
        Utilities.Delay(3, ()=> ResetCursorTransform(), this);
        OnFlagChanged();
    }
}
