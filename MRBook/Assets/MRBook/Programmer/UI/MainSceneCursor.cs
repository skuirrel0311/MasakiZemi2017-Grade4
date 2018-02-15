using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneCursor : MyCursor
{
    [SerializeField]
    int actorLayer = 8;

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

        //矢印付きのカーソル
        isRecognizedPosCon = true;
        OnFlagChanged();
    }
}
