﻿using UnityEngine;

/// <summary>
/// アイテムの受け皿
/// </summary>
public class BaseItemSaucer : MonoBehaviour
{
    protected HoloObject owner = null;

    public bool IsVisuable { get; protected set; }

    public virtual void Init(HoloObject owner)
    {
        this.owner = owner;
    }

    public virtual void SetItem(HoloItem item, bool showParticle = true) { }
    public virtual bool CheckCanHaveItem(HoloItem item) { return false; }
    /// <summary>
    /// 持っている全てのアイテムを捨てる
    /// </summary>
    public virtual void DumpItem() { }
    //指定されたアイテムを捨てる
    public virtual void DumpItem(HoloItem item) { }

    public virtual bool Equals(GameObject other) { return false; }

    public virtual void Show() { }
    public virtual void Close() { }
}

public class CharacterItemSaucer : BaseItemSaucer
{
    HoloCharacter ownerCharacter;

    HandIconController handIconController = null;

    //アルコールを摂取したか？
    public bool IsGetAlcohol { get; private set; }
    const string AlcoholItemName = "Sakabin";

    public bool HasItem_Left { get { return LeftHandItem != null; } }
    public bool HasItem_Right { get { return RightHandItem != null; } }

    public HoloItem LeftHandItem { get; private set; }
    public HoloItem RightHandItem { get; private set; }
    [SerializeField]
    Transform rightHand = null;
    [SerializeField]
    Transform leftHand = null;

    [SerializeField]
    ItemTransformDataList rightHandItemDataList = null;
    [SerializeField]
    ItemTransformDataList leftHandItemDataList = null;

    ItemTransformDataList itemList;
    ItemTransformData itemData;
    Transform hand;
    
    public override void Init(HoloObject owner)
    {
        ownerCharacter = (HoloCharacter)owner;

        if (handIconController != null) return;
        handIconController = ActorManager.I.handIconControllerPrefab;
        handIconController = Instantiate(handIconController, owner.transform);

        Vector3 iconPosition = handIconController.transform.position;
        iconPosition.y += 1.2f * owner.transform.lossyScale.y;
        handIconController.transform.position = iconPosition;
        float scale = 1.0f /  handIconController.transform.lossyScale.x;
        handIconController.transform.localScale = Vector3.one * scale;

        handIconController.Init(this);
    }

    /// <summary>
    /// そのアイテムは持てるのかチェックする
    /// </summary>
    public override bool CheckCanHaveItem(HoloItem item)
    {
        itemList = GetItemTransformDataList(item);
        hand = itemList.hand == HoloItem.Hand.Left ? leftHand : rightHand;
        if (hand == null)
        {
            return false;
        }

        itemData = GetItemTransformDate(item.name, itemList);
        if (itemData == null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// アイテムを持たせる
    /// </summary>
    public override void SetItem(HoloItem item, bool showParticle = true)
    {
        if (!CheckCanHaveItem(item)) return;
        
        if (itemList.hand == HoloItem.Hand.Left)
        {
            item.currentHand = HoloItem.Hand.Left;
            LeftHandItem = item;
        }
        else
        {
            item.currentHand = HoloItem.Hand.Right;
            RightHandItem = item;
        }

        item.owner = ownerCharacter;
        item.transform.parent = hand;
        item.transform.localPosition = itemData.position;
        item.transform.localRotation = itemData.rotation;

        if (item.name == AlcoholItemName)
        {
            IsGetAlcohol = true;
        }

        //モーションを変える
        handIconController.Hide();
        AkSoundEngine.PostEvent("Equid", gameObject);
        if(showParticle)  ParticleManager.I.Play("Doron", transform.position, Quaternion.identity);
        ownerCharacter.ChangeAnimationClip(itemData.motionName, 0.0f);
    }

    /// <summary>
    /// 指定された手に持っているアイテムを捨てる
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="setDefaultTransform">捨てたアイテムを元の位置に戻すか？</param>
    void DumpItem(HoloItem.Hand hand)
    {
        if (hand == HoloItem.Hand.Both)
        {
            DumpItem(HoloItem.Hand.Left);
            DumpItem(HoloItem.Hand.Right);
            return;
        }

        HoloItem oldItem;
        if (hand == HoloItem.Hand.Right)
        {
            oldItem = RightHandItem;
            RightHandItem = null;
        }
        else
        {
            oldItem = LeftHandItem;
            LeftHandItem = null;
        }

        if (oldItem == null) return;

        //ドロップする
        ItemDropper.I.Drop(oldItem.owner, oldItem);
        oldItem.owner = null;

        if (oldItem.name == AlcoholItemName)
        {
            IsGetAlcohol = false;
        }
    }
    
    public override void DumpItem()
    {
        //両手に持っているアイテムを捨てる
        DumpItem(HoloItem.Hand.Both);
    }

    public override void DumpItem(HoloItem item)
    {
        DumpItem(item.ForHand);

        Show();
    }

    ItemTransformData GetItemTransformDate(string name, ItemTransformDataList list)
    {
        //todo:できればDictionaryに変換したいがScriptableObjectと相性が悪いので放置
        if (list == null)
        {
            Debug.LogError("list is null");
            return null;
        }
        for (int i = 0; i < list.dataList.Count; i++)
        {
            if (list.dataList[i].itemName == name) return list.dataList[i];
        }
        return null;
    }

    ItemTransformDataList GetItemTransformDataList(HoloItem item)
    {
        switch (item.ForHand)
        {
            case HoloItem.Hand.Left:
                //持っていたら捨てる
                if (HasItem_Left) DumpItem(HoloItem.Hand.Left);
                return leftHandItemDataList;

            case HoloItem.Hand.Right:
                //持っていたら捨てる
                if (HasItem_Right) DumpItem(HoloItem.Hand.Right);
                return rightHandItemDataList;

            case HoloItem.Hand.Both:
                if (HasItem_Left && HasItem_Right)
                {
                    //どちらの手にもアイテムを持っている
                    //todo:UIでどちらの手に持っているアイテムを捨てるか表示
                    //とりあえず右のアイテムを捨てる
                    DumpItem(HoloItem.Hand.Right);
                    return rightHandItemDataList;
                }
                //どちらにも持っていなかったら右になる
                return HasItem_Right ? leftHandItemDataList : rightHandItemDataList;
            default:
                return null;
        }
    }

    public override void Show()
    {
        IsVisuable = true;
        handIconController.Show();
    }

    public override void Close()
    {
        IsVisuable = false;
        handIconController.Hide();
    }

    public override bool Equals(GameObject other)
    {
        bool equal = gameObject.Equals(other);

        if (!equal && HasItem_Left) equal = LeftHandItem.gameObject.Equals(other);
        if (!equal && HasItem_Right) equal = RightHandItem.gameObject.Equals(other);

        return equal;
    }
}
