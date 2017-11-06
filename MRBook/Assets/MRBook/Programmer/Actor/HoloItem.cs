using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloItem : HoloGroundingObject
{
    public enum Hand { Right, Left, Both }

    /// <summary>
    /// どちらの手で持つアイテムなのか？
    /// </summary>
    public Hand hand;

    public override Type GetActorType { get { return Type.Item; } }
    [System.NonSerialized]
    public HoloCharacter owner;
    [System.NonSerialized]
    public Hand currentHand;

    public override void ResetTransform()
    {
        //オーナー側でリセットを呼ぶ
        if (owner != null) return;
        base.ResetTransform();
    }
}
