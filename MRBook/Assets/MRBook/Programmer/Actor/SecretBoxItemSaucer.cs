using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玉手箱が持っている受け皿
/// </summary>
public class SecretBoxItemSaucer : BaseItemSaucer
{
    public override void Init(HoloObject owner)
    {
        base.Init(owner);

        AddBehaviour(new SecretBoxItemSaucerBehaviour(owner, (HoloItem)owner));
    }

    public override void SetItem(HoloItem item, bool showParticle = true)
    {
        behaviour.OnSetItem(item, showParticle);
    }

    public override bool CheckCanHaveItem(HoloItem item)
    {
        return true;
    }

    public override void DumpItem(bool isDrop = true)
    {
        behaviour.OnDumpItem();
    }
    public override void DumpItem(HoloItem item, bool isDrop = true)
    {
        behaviour.OnDumpItem(item);
    }
}
