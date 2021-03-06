﻿using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 背景・小物などの動かないホログラム
/// </summary>
public class HoloObject : MonoBehaviour
{
    public enum Type { Character, Item, Statics }
    public virtual Type GetActorType { get { return Type.Statics; } }

    /// <summary>
    /// そのオブジェクトが存在する（元の）ページのインデックス
    /// </summary>
    public int PageIndex { get; private set; }
    bool isFirst = true;
    public Transform defaultParent { get; protected set; }

    [SerializeField]
    bool isMovable = false;
    [SerializeField]
    bool canHaveItem = false;
    [SerializeField]
    int inPlayLayer = 4;

    [SerializeField]
    string overrideName = "";
    
    public BaseObjInputHandler InputHandler { get; private set; }
    public BaseItemSaucer ItemSaucer { get; private set; }

    protected HoloObjResetter resetter = null;
    public HoloObjResetter Resetter
    {
        get
        {
            if (resetter != null) return resetter;
            resetter = GetResetterInstance();
            return resetter;
        }
    }

    protected virtual HoloObjResetter GetResetterInstance() { return new HoloObjResetter(); }

    /// <summary>
    /// ページが開かれた
    /// </summary>
    /// <param name="isFirst">そのページを開くのが初めてか？</param>
    public virtual void PageStart(int currentPageIndex)
    {
        if (isFirst)
        {
            PageIndex = currentPageIndex;
            Init();
        }
        isFirst = false;
    }

    protected virtual void Init()
    {
        InitResetter();
        
        if(isMovable)
        {
            InputHandler = GetComponent<BaseObjInputHandler>();
        }

        if (InputHandler != null) InputHandler.Init(this);

        InitItemSaucer();

        SetDefaultParent();
    }

    protected virtual void SetDefaultParent()
    {
        defaultParent = transform.parent;
    }

    public void InitItemSaucer()
    {
        if(canHaveItem)
        {
            ItemSaucer = GetComponent<BaseItemSaucer>();
        }
        if (ItemSaucer != null) ItemSaucer.Init(this);
    }

    protected virtual void InitResetter()
    {
        Resetter.AddBehaviour(new DefaultHoloObjResetBehaviour(this));

        //スタティックなのにムーバブルなときはリセットの挙動に位置も追加する
        if(GetActorType == Type.Statics && isMovable)
        {
            Resetter.AddBehaviour(new LocationResetBehaviour(this));
        }
    }

    public virtual void PlayPage()
    {
        gameObject.layer = inPlayLayer;
    }

    public bool CheckCanHaveItem(HoloItem item)
    {
        if (ItemSaucer == null) return false;

        return ItemSaucer.CheckCanHaveItem(item);
    }

    public void SetItem(HoloItem item)
    {
        if (!CheckCanHaveItem(item)) return;

        //CheckCanHaveItemがtrueになったということはitemSaucerはnullではない
        ItemSaucer.SetItem(item);
    }

    public virtual void ChangeScale(float scaleRate)
    {
        transform.localScale = transform.localScale * scaleRate;
    }

    /// <summary>
    /// アイテムを所有している場合はアイテムも自身とカウントする
    /// </summary>
    public bool Equals(GameObject other)
    {
        if (ItemSaucer == null) return gameObject.Equals(other);
        bool equal = false;
        equal = gameObject.Equals(other);
        if (!equal) equal = ItemSaucer.Equals(other);

        return equal;
    }

    public string GetName()
    {
        if (string.IsNullOrEmpty(overrideName)) return name;

        return overrideName;
    }

    public virtual void SetParent(Transform parent)
    {
        transform.parent = parent;
    }
}
