using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人型のホログラム(アイテムをもったりできる)
/// </summary>
public class HoloCharacter : HoloActor
{
    /// <summary>
    /// アイテムを持つことが出来るか？
    /// </summary>
    public bool canHasItem = false;
    //public bool hasItem_Left { get; private set; }
    //public bool hasItem_Right { get; private set; }

    public bool hasItem_Left;
    public bool hasItem_Right;

    [SerializeField]
    Transform rightHand = null;
    [SerializeField]
    Transform leftHand = null;

    [SerializeField]
    ItemTransformDataList rightHandItemDataList = null;
    [SerializeField]
    ItemTransformDataList leftHandItemDataList = null;

    public override ActorType GetActorType { get { return ActorType.Character; } }

    void Start()
    {
        if(rightHandItemDataList != null && rightHand != null) rightHandItemDataList.parent = rightHand;
        if(leftHandItemDataList != null && leftHand != null) leftHandItemDataList.parent = leftHand;
    }

    /// <summary>
    /// アイテムを持たせる
    /// </summary>
    public override void SetItem(GameObject itemObj)
    {

        HoloItem item = itemObj.GetComponent<HoloItem>();

        if (item == null) return;

        ItemTransformDataList itemList = GetItemTransformDataList(item);
        ItemTransformData itemData = GetItemTransformDate(itemObj.name, itemList);

        if (itemData == null) return;
        if (itemList.parent == null) return;

        if (itemList.parent.Equals(leftHand)) hasItem_Left = true;
        else hasItem_Right = true;

        Debug.Log("set current hand");
        item.currentHand = hasItem_Left ? HoloItem.Hand.Left : HoloItem.Hand.Right;

        item.owner = this;
        item.transform.parent = itemList.parent;
        item.transform.localPosition = itemData.position;
        item.transform.localRotation = itemData.rotation;
    }

    ItemTransformData GetItemTransformDate(string name, ItemTransformDataList list)
    {
        if (list == null) return null;
        for (int i = 0; i < list.dataList.Count; i++)
        {
            if (list.dataList[i].itemName == name) return list.dataList[i];
        }
        return null;
    }

    ItemTransformDataList GetItemTransformDataList(HoloItem item)
    {
        if (item.hand == HoloItem.Hand.Both)
        {
            if (hasItem_Left && hasItem_Right)
            {
                //どちらの手にもアイテムを持っている
                //todo:UIでどちらの手に持っているアイテムを捨てるか表示
                //とりあえず右のアイテムを捨てる
                DumpItem(HoloItem.Hand.Right);
                return rightHandItemDataList;
            }
        }

        //既に持っていたら捨てる
        if (hasItem_Left && item.hand == HoloItem.Hand.Left) DumpItem(HoloItem.Hand.Left);
        if (hasItem_Right && item.hand == HoloItem.Hand.Right) DumpItem(HoloItem.Hand.Right);


        if (item.hand == HoloItem.Hand.Left) return leftHandItemDataList;
        if (item.hand == HoloItem.Hand.Right) return rightHandItemDataList;

        //Bothの場合どちらにも持っていなかったら右になる
        return hasItem_Right ? leftHandItemDataList : rightHandItemDataList;
    }

    /// <summary>
    /// 指定された手に持っているアイテムを捨てる
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="setDefault">捨てたアイテムを元の位置に戻すか？</param>
    public void DumpItem(HoloItem.Hand hand, bool setDefault = true)
    {
        //その前に持っていたアイテムは初期座標に戻す
        if (hand == HoloItem.Hand.Both)
        {
            DumpItem(HoloItem.Hand.Left, setDefault);
            DumpItem(HoloItem.Hand.Right, setDefault);
            hasItem_Left = false;
            hasItem_Right = false;
            return;
        }
        
        HoloItem oldItem;
        if (hand == HoloItem.Hand.Right)
        {
            hasItem_Right = false;
            oldItem = rightHand.GetComponentInChildren<HoloItem>();
        }
        else
        {
            hasItem_Left = false;
            oldItem = leftHand.GetComponentInChildren<HoloItem>();
        }

        if (!setDefault) return;

        if (oldItem != null)
        {
            oldItem.owner = null;
            oldItem.transform.parent = null;
            //todo:グローバルの場所に戻る可能性も考えるべき
            oldItem.ResetTransform();
        }
    }
}
