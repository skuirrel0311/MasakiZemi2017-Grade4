using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloItem : HoloActor
{
    public enum Hand { Right, Left, Both }

    /// <summary>
    /// どちらの手で持つアイテムなのか？
    /// </summary>
    public Hand hand;

    public override ActorType GetActorType { get { return ActorType.Item; } }
    [System.NonSerialized]
    public HoloCharacter owner;
    [System.NonSerialized]
    public Hand currentHand;
}
