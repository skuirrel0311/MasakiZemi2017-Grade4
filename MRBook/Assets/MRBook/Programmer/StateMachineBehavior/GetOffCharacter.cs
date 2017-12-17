using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class GetOffCharacter : BaseStateMachineBehaviour
{
    [SerializeField]
    ActorName rideCharacterName = ActorName.Urashima;
    [SerializeField]
    ActorName matCharacterName = ActorName.Turtle;
    [SerializeField]
    string rideEndPointName = "";

    protected override void OnStart()
    {
        base.OnStart();

        HoloCharacter rideCharacter = ActorManager.I.GetCharacter(rideCharacterName);
        HoloCharacter matCharacter = ActorManager.I.GetCharacter(matCharacterName);
        Transform rideEndPoint = ActorManager.I.GetTargetPoint(rideEndPointName);

        rideCharacter.transform.localPosition = rideEndPoint.localPosition;
        rideCharacter.transform.localRotation = rideEndPoint.localRotation;

        Utilities.Delay(1, () =>
        {
            rideCharacter.transform.parent = matCharacter.transform.parent;
            if(rideCharacter.m_agent != null) rideCharacter.m_agent.enabled = true;
        }, rideCharacter);

    }
}
