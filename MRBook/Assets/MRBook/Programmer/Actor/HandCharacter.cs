using UnityEngine;

//アイテムを持つことのできるキャラクター
public class HandCharacter : HoloCharacter
{
    //アルコールを摂取したか？
    public bool IsGetAlcohol { get; private set; }
    const string AlcoholItemName = "Sakabin";

    public override bool CanHaveItem { get { return true; } }

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

    /// <summary>
    /// そのアイテムは持てるのかチェックする
    /// </summary>
    public bool CheckCanHaveItem(HoloItem item)
    {
        itemList = GetItemTransformDataList(item);
        hand = itemList.hand == HoloItem.Hand.Left ? leftHand : rightHand;
        if (hand == null)
        {
            return false;
        }

        itemData = GetItemTransformDate(item.name, itemList);
        if(itemData == null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// アイテムを持たせる
    /// </summary>
    public void SetItem(HoloItem item)
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

        ChangeAnimationClip(itemData.motionName, 0.0f);
    }

    /// <summary>
    /// 指定された手に持っているアイテムを捨てる
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="setDefaultTransform">捨てたアイテムを元の位置に戻すか？</param>
    public void DumpItem(HoloItem.Hand hand, bool setDefaultTransform = true)
    {
        if (hand == HoloItem.Hand.Both)
        {
            DumpItem(HoloItem.Hand.Left, setDefaultTransform);
            DumpItem(HoloItem.Hand.Right, setDefaultTransform);
            return;
        }

        HoloItem oldItem;
        if (hand == HoloItem.Hand.Right)
        {
            oldItem = rightHand.GetComponentInChildren<HoloItem>();
            RightHandItem = null;
        }
        else
        {
            oldItem = leftHand.GetComponentInChildren<HoloItem>();
            LeftHandItem = null;
        }

        if (oldItem == null) return;

        oldItem.owner = null;
        //todo:parentをnullにするのはダメ
        oldItem.transform.parent = null;
        //todo:ResetManagerに単体でリセットする機能を実装する
        //if (setDefault) oldItem.ResetTransform();
        //else //todo:ドロップする

        if (oldItem.name == AlcoholItemName)
        {
            IsGetAlcohol = false;
        }
    }

    public override void ChangeAnimationClip(MotionName name, float transitionDuration)
    {
        if (m_animator == null) return;

        string animationName = MotionNameManager.GetMotionName(name, this);
        m_animator.CrossFade(animationName, transitionDuration);
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

    //持っているアイテムもEqualの対象にする
    public override bool Equals(object other)
    {
        bool equal = gameObject.Equals(other);

        if (!equal && HasItem_Left) equal = LeftHandItem.gameObject.Equals(other);
        if (!equal && HasItem_Right) equal = RightHandItem.gameObject.Equals(other);

        return equal;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
