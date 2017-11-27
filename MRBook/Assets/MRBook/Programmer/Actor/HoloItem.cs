using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloItem : HoloMovableObject
{
    public enum Hand { Right, Left, Both }

    public override Type GetActorType { get { return Type.Item; } }

    /// <summary>
    /// どちらの手で持つアイテムなのか？
    /// </summary>
    [SerializeField]
    Hand forHand = Hand.Right;
    public Hand ForHand { get { return forHand; } }
    
    //アイテムの所持者
    [System.NonSerialized]
    public HandCharacter owner;
    //どちらの手に持たれているか
    [System.NonSerialized]
    public Hand currentHand;

    int defaultLayer;

    protected void Awake()
    {
        defaultLayer = gameObject.layer;
    }

    public override void PlayPage()
    {
        base.PlayPage();
        //オーナーがパペットマスターの制御下にある場合にWaterLayerに変更すると困るので
        if (owner != null) gameObject.layer = defaultLayer;
    }

    protected override void InitResetter()
    {
        base.InitResetter();
        resetter.AddBehaviour(new LocationResetBehaviour(this));
        resetter.AddBehaviour(new ItemResetBehaviour(this));
    }
}
