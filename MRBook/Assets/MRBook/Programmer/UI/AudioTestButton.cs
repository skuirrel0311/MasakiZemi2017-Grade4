using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTestButton : HoloButton
{
    [SerializeField]
    string triggerName = "";
    //音が鳴る地点（nullだったら自身が代入される）
    [SerializeField]
    GameObject point = null;

    protected override void Start()
    {
        base.Start();

        if (point == null) point = gameObject;
        onClick.AddListener(() =>
        {
            AkSoundEngine.PostEvent(triggerName, point);
        });

    }

}
