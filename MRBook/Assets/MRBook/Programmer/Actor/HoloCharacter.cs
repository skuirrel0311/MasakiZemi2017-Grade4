using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムをもったりできるホログラム
/// </summary>
public class HoloCharacter : HoloGroundingObject
{
    public Animator m_animator { get; private set; }
    [SerializeField]
    protected MotionName firstAnimationName = MotionName.Wait;
    
    public bool IsGetAlcohol { get; private set; }
    const string AlcoholItemName = "Sakabin";

    [SerializeField]
    bool canHaveItem = false;
    public bool hasItem_Left { get; private set; }
    public bool hasItem_Right { get; private set; }

    public HoloItem leftHandItem { get; private set; }
    public HoloItem rightHandItem { get; private set; }
    [SerializeField]
    Transform rightHand = null;
    [SerializeField]
    Transform leftHand = null;

    [SerializeField]
    ItemTransformDataList rightHandItemDataList = null;
    [SerializeField]
    ItemTransformDataList leftHandItemDataList = null;

    public override Type GetActorType { get { return Type.Character; } }

    protected override void Awake()
    {
        m_animator = GetComponent<Animator>();
        if (m_animator != null) m_animator.CrossFade(MotionNameManager.GetMotionName(firstAnimationName, this), 0.1f);
        base.Awake();
    }

    /// <summary>
    /// アイテムを持たせる
    /// </summary>
    public override void SetItem(GameObject itemObj)
    {
        if (!canHaveItem) return;
        if (itemObj == null)
        {
            Debug.LogError("item obj is null");
        }
        HoloItem item = itemObj.GetComponent<HoloItem>();

        if (item == null)
        {
            Debug.LogError("item is null");
            return;
        }
        ItemTransformDataList itemList = GetItemTransformDataList(item);
        ItemTransformData itemData = GetItemTransformDate(itemObj.name, itemList);


        if (itemData == null)
        {
            Debug.LogError("item data is null");
            return;
        }
        Transform hand = itemList.hand == HoloItem.Hand.Left ? leftHand : rightHand;

        if (hand == null)
        {
            Debug.LogError("hand is null");
            return;
        }


        if (itemList.hand == HoloItem.Hand.Left)
        {
            hasItem_Left = true;
            item.currentHand = HoloItem.Hand.Left;
            leftHandItem = item;
        }
        else
        {
            hasItem_Right = true;
            item.currentHand = HoloItem.Hand.Right;
            rightHandItem = item;
        }

        item.owner = this;
        item.transform.parent = hand;
        item.transform.localPosition = itemData.position;
        item.transform.localRotation = itemData.rotation;

        //モーションを変える
        ParticleManager.I.Play("Doron", transform.position, Quaternion.identity);

        if (item.name == AlcoholItemName)
        {
            IsGetAlcohol = true;
        }

        m_animator.CrossFade(MotionNameManager.GetMotionName(itemData.motionName, this), 0.0f);
    }

    /// <summary>
    /// 指定された手に持っているアイテムを捨てる
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="setDefault">捨てたアイテムを元の位置に戻すか？</param>
    public void DumpItem(HoloItem.Hand hand, bool setDefault = true)
    {
        if (!canHaveItem) return;
        //その前に持っていたアイテムは初期座標に戻す
        if (hand == HoloItem.Hand.Both)
        {
            DumpItem(HoloItem.Hand.Left, setDefault);
            DumpItem(HoloItem.Hand.Right, setDefault);
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

        if (oldItem == null) return;

        oldItem.owner = null;
        //todo:parentをnullにするのはダメ
        oldItem.transform.parent = null;
        //todo:グローバルの場所に戻る可能性も考えるべき
        if (setDefault) oldItem.ResetTransform();

        if (oldItem.name == AlcoholItemName)
        {
            IsGetAlcohol = false;
        }
    }

    public override void ResetTransform()
    {
        //アイテムもリセット
        DumpItem(HoloItem.Hand.Both);
        gameObject.SetActive(true);
        //ページが開始された時のモーションに戻す
        m_animator.CrossFade(MotionNameManager.GetMotionName(firstAnimationName, this), 0.0f);
        base.ResetTransform();
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
        switch (item.hand)
        {
            case HoloItem.Hand.Left:
                //持っていたら捨てる
                if (hasItem_Left) DumpItem(HoloItem.Hand.Left);
                return leftHandItemDataList;

            case HoloItem.Hand.Right:
                //持っていたら捨てる
                if (hasItem_Right) DumpItem(HoloItem.Hand.Right);
                return rightHandItemDataList;

            case HoloItem.Hand.Both:
                if (hasItem_Left && hasItem_Right)
                {
                    //どちらの手にもアイテムを持っている
                    //todo:UIでどちらの手に持っているアイテムを捨てるか表示
                    //とりあえず右のアイテムを捨てる
                    DumpItem(HoloItem.Hand.Right);
                    return rightHandItemDataList;
                }
                //どちらにも持っていなかったら右になる
                return hasItem_Right ? leftHandItemDataList : rightHandItemDataList;
            default:
                return null;
        }
    }

    public override bool Equals(object other)
    {
        bool equal = gameObject.Equals(other);

        if (!equal && hasItem_Left) equal = leftHandItem.gameObject.Equals(other);
        if (!equal && hasItem_Right) equal = rightHandItem.gameObject.Equals(other);

        return equal;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
