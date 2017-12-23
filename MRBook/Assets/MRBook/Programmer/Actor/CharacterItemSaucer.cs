using UnityEngine;
using KKUtilities;

/// <summary>
/// アイテムの受け皿
/// </summary>
public class BaseItemSaucer : MonoBehaviour
{
    protected HoloObject owner = null;
    public HoloObject Owner { get { return owner; } }

    protected BaseItemSaucerBehaviour behaviour = null;

    public virtual void Init(HoloObject owner)
    {
        this.owner = owner;
    }

    public virtual void SetItem(HoloItem item, bool showParticle = true) { }
    public virtual bool CheckCanHaveItem(HoloItem item) { return false; }
    /// <summary>
    /// 持っている全てのアイテムを捨てる
    /// </summary>
    public virtual void DumpItem(bool isDrop = true) { }
    //指定されたアイテムを捨てる
    public virtual void DumpItem(HoloItem item, bool isDrop = true) { }

    public void AddBehaviour(BaseItemSaucerBehaviour behaviour)
    {
        Debug.Log("call add behaviour");
        this.behaviour = behaviour;
    }
    public void RemoveBehaviour()
    {
        behaviour = null;
    }

    public virtual bool Equals(GameObject other) { return false; }
}

public class CharacterItemSaucer : BaseItemSaucer
{
    HoloCharacter ownerCharacter;

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

    const string SecretBoxName = "SecretBox_Box";
    
    public override void Init(HoloObject owner)
    {
        ownerCharacter = (HoloCharacter)owner;
        base.Init(owner);
    }

    /// <summary>
    /// そのアイテムは持てるのかチェックする
    /// </summary>
    public override bool CheckCanHaveItem(HoloItem item)
    {
        if (behaviour != null)
        {
            return behaviour.CheckCanHaveItem(item);
        }

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
        if(behaviour != null)
        {
            behaviour.OnSetItem(item, showParticle);
            return;
        }
         
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
        item.transform.localEulerAngles = itemData.rotation;
        item.SetColliderEnable(false);

        if (item.name == AlcoholItemName)
        {
            IsGetAlcohol = true;
        }

        if(item.name == SecretBoxName)
        {
            AddBehaviour(new SecretBoxItemSaucerBehaviour(owner, item));
        }

        //モーションを変える
        HandIconController.I.Hide();
        AkSoundEngine.PostEvent("Equip", gameObject);
        if (showParticle) ParticleManager.I.Play("Doron", owner.transform.position, Quaternion.identity);
        ownerCharacter.ChangeAnimationClip(itemData.motionName, 0.0f);
    }

    /// <summary>
    /// 指定された手に持っているアイテムを捨てる
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="setDefaultTransform">捨てたアイテムを元の位置に戻すか？</param>
    void DumpItem(HoloItem.Hand hand, bool isDrop = true)
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
        if(isDrop) ItemDropper.I.Drop(oldItem.owner, oldItem);
        oldItem.owner = null;
        oldItem.SetColliderEnable(true);

        if (oldItem.name == SecretBoxName)
        {
            RemoveBehaviour();
        }

        Utilities.Delay(0.11f, () =>
        {
            HandIconController.I.Hide();
        },owner);


        if (oldItem.name == AlcoholItemName)
        {
            IsGetAlcohol = false;
        }
    }
    
    public override void DumpItem(bool isDrop = true)
    {
        //両手に持っているアイテムを捨てる
        DumpItem(HoloItem.Hand.Both, isDrop);
    }

    public override void DumpItem(HoloItem item, bool isDrop = true)
    {
        if (item == null) return;
        DumpItem(item.ForHand, isDrop);
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

    public override bool Equals(GameObject other)
    {
        bool equal = gameObject.Equals(other);

        if (!equal && HasItem_Left) equal = LeftHandItem.gameObject.Equals(other);
        if (!equal && HasItem_Right) equal = RightHandItem.gameObject.Equals(other);

        return equal;
    }
}
