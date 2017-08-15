using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloItem : HoloMovableObject
{
    public enum Hand { Right, Left, Both }

    /// <summary>
    /// どちらの手で持つアイテムなのか？
    /// </summary>
    public Hand hand;

    public override HoloObjectType GetActorType { get { return HoloObjectType.Item; } }
    [System.NonSerialized]
    public HoloCharacter owner;
    [System.NonSerialized]
    public Hand currentHand;
}
