using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretBoxItemSaucer : BaseItemSaucer
{
    List<HoloItem> contentItemList = new List<HoloItem>();

    //蓋
    const string LidItemName = "SecretBox_Lid";

    //箱の中にものをつめる(開いている必要がある？)
    public override void SetItem(HoloItem item, bool showParticle = true)
    {
        HandIconController.I.Hide();
        AkSoundEngine.PostEvent("Equid", gameObject);
        if (showParticle) ParticleManager.I.Play("Doron", transform.position, Quaternion.identity);

        if (item.name == LidItemName)
        {
            //蓋は箱と同じ位置に配置すればしまっているように見える
            item.transform.parent = transform;
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;

            return;
        }

        contentItemList.Add(item);
        item.gameObject.SetActive(false);
    }

    public override bool CheckCanHaveItem(HoloItem item)
    {
        return true;
    }
}
