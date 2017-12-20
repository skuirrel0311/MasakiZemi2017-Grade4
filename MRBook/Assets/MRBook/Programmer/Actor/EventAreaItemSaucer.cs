using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// アイテムがエリアに配置された時にイベントを発生させる
/// </summary>
public class EventAreaItemSaucer : BaseItemSaucer
{
    [SerializeField]
    UnityEvent onSetItem = null;

    [SerializeField]
    string[] acceptObjectNames = null;

    public override void SetItem(HoloItem item, bool showParticle = true)
    {
        if (onSetItem != null) onSetItem.Invoke();
        if (showParticle) ParticleManager.I.Play("Doron", owner.transform.position, Quaternion.identity);
    }

    public override bool CheckCanHaveItem(HoloItem item)
    {
        for(int i = 0;i< acceptObjectNames.Length;i++)
        {
            if (item.name == acceptObjectNames[i]) return true;
        }

        return false;
    }
}
