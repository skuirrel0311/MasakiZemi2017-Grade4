using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class RideCharacter : BaseStateMachineBehaviour
{
    [SerializeField]
    ActorName rideCharacterName = ActorName.Urashima;
    [SerializeField]
    ActorName matCharacterName = ActorName.Turtle;
    [SerializeField]
    string ridePointName = "";

    protected override void OnStart()
    {
        base.OnStart();

        HoloCharacter rideCharacter = ActorManager.I.GetCharacter(rideCharacterName);
        HoloCharacter matCharacter = ActorManager.I.GetCharacter(matCharacterName);
        Transform ridePoint = ActorManager.I.GetTargetPoint(ridePointName);

        rideCharacter.transform.parent = matCharacter.transform;
        rideCharacter.m_agent.enabled = false;

        Utilities.Delay(1, () =>
        {
            rideCharacter.transform.localPosition = ridePoint.localPosition;
            rideCharacter.transform.localRotation = ridePoint.localRotation;
        }, rideCharacter);

    }
}
