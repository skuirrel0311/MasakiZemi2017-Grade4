using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HoloItem : HoloObject
{
    public enum Hand { Right, Left, Both }

    public override Type GetActorType { get { return Type.Item; } }
    protected override HoloObjResetter GetResetterInstance() { return new HoloMovableObjResetter(this); }
    
    public float itemTextHeight = 0.0f;

    //見つめてると表示されるテキスト（日本語）
    public string nameText = "";
    //アイテムの説明テキスト
    [Multiline]
    public string explanatoryText = "";

    [SerializeField]
    Hand forHand = Hand.Right;
    /// <summary>
    /// どちらの手で持つアイテムなのか？
    /// </summary>
    public Hand ForHand { get { return forHand; } }

    [SerializeField]
    HoloObject defaultOwner = null;

    //アイテムの所持者
    //[System.NonSerialized]
    public HoloObject owner;
    //どちらの手に持たれているか
    [System.NonSerialized]
    public Hand currentHand;

    int defaultLayer;

    Collider[] cols;

    protected void Awake()
    {
        defaultLayer = gameObject.layer;

        cols = GetComponents<Collider>();
    }

    void Start()
    {
        if (defaultOwner == null) return;
        if (defaultOwner.name == "Urashima")
        {
            ((CharacterItemSaucer)defaultOwner.ItemSaucer).SetItem(this, false, false);
            return;
        }
        defaultOwner.ItemSaucer.SetItem(this, false);
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

    public void SetColliderEnable(bool enabled)
    {
        if (cols == null) return;

        for(int i = 0;i< cols.Length;i++)
        {
            cols[i].enabled = enabled;
        }
    }
}
