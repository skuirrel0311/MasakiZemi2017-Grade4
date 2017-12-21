using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class GetOffCharacter : BaseStateMachineBehaviour
{
    [SerializeField]
    string rideObjectName = "";
    [SerializeField]
    string matObjectName = "";
    [SerializeField]
    string rideEndPointName = "";

    protected override void OnStart()
    {
        base.OnStart();

        HoloObject rideObject = ActorManager.I.GetObject(rideObjectName);
        HoloObject matObject = ActorManager.I.GetObject(matObjectName);
        Transform rideEndPoint = ActorManager.I.GetTargetPoint(rideEndPointName);

        rideObject.gameObject.SetActive(false);

        //1フレーム待ってから移動させる
        Utilities.Delay(1, () =>
        {
            rideObject.SetParent(matObject.transform.parent);
            rideObject.transform.position = rideEndPoint.position;
            rideObject.transform.rotation = rideEndPoint.rotation;
            rideObject.gameObject.SetActive(true);
        }, StateMachineManager.I);

    }
}
