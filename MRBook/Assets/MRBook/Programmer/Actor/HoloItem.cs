using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class HoloItem : HoloObject
{
    public enum Hand { Right, Left, Both }

    public override Type GetActorType { get { return Type.Item; } }
    protected override HoloObjResetter GetResetterInstance() { return new HoloMovableObjResetter(this); }
    
    [SerializeField]
    Hand forHand = Hand.Right;
    /// <summary>
    /// どちらの手で持つアイテムなのか？
    /// </summary>
    public Hand ForHand { get { return forHand; } }

    [SerializeField]
    HoloObject defaultOwner = null;

    public Transform defaultParent { get; private set; }

    //アイテムの所持者
    [System.NonSerialized]
    public HoloObject owner;
    //どちらの手に持たれているか
    [System.NonSerialized]
    public Hand currentHand;

    int defaultLayer;

    protected void Awake()
    {
        defaultLayer = gameObject.layer;
        defaultParent = transform.parent;
    }

    void Start()
    {
        if (defaultOwner == null) return;
        defaultOwner.ItemSaucer.SetItem(this, false);
    }

    protected override void Init()
    {
        HoloObjResetManager.I.AddMovableResetter((HoloMovableObjResetter)Resetter);
        base.Init();
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
        Resetter.AddBehaviour(new ItemResetBehaviour(this, defaultOwner));
    }
}
