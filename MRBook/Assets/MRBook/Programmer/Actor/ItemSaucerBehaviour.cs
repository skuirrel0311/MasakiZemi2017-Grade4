using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemSaucerBehaviour
{
    public enum BehaviourType {
        Override,       //完全に上書き
        And,            //どちらも持てれば持つ
        Or              //どちらかが持てれば持つ
    }

    protected HoloObject owner;

    public virtual BehaviourType GetBehaviourType { get { return BehaviourType.Or; } }

    public BaseItemSaucerBehaviour(HoloObject owner)
    {
        this.owner = owner;
    }
    public virtual bool CheckCanHaveItem(HoloItem item) { return false; }
    public virtual void OnSetItem(HoloItem item, bool showParticle = true) { }
    public virtual void OnDumpItem() { }
    public virtual void OnDumpItem(HoloItem item) { }
}

//玉手箱だったり、玉手箱を持った人に追加される挙動
public class SecretBoxItemSaucerBehaviour : BaseItemSaucerBehaviour
{
    public override BehaviourType GetBehaviourType { get { return BehaviourType.Override; } }
    HoloItem box;
    //蓋
    const string LidItemName = "SecretBox_Lid";
    
    //ownerが箱である確証はない
    public SecretBoxItemSaucerBehaviour(HoloObject owner, HoloItem box)
        :base(owner)
    {
        this.box = box;
    }

    public override bool CheckCanHaveItem(HoloItem item)
    {
        //来るもの拒まず
        return true;
    }

    public override void OnSetItem(HoloItem item, bool showParticle = true)
    {
        if(showParticle)  ParticleManager.I.Play("Doron", owner.transform.position, Quaternion.identity);
        
        if (item.name == LidItemName)
        {
            //蓋は箱と同じ位置に配置すればしまっているように見える
            item.transform.parent = box.transform;
            item.owner = owner;
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            item.SetColliderEnable(false);
            return;
        }

        item.gameObject.SetActive(false);
        ResultManager.I.AddSecretBoxContent(item);
    }

    public override void OnDumpItem()
    {
        ResultManager.I.RemoveAllSecretBoxContents();
    }

    public override void OnDumpItem(HoloItem item)
    {
        ResultManager.I.RemoveSecretBoxContent(item);
    }
}
