using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//浦島側で死因が計測できなかった場合や、特別に死因を決めたい場合に使用する
public class OverrideEnding : BaseStateMachineBehaviour
{
    [SerializeField]
    string endingName = "顔面強打";

    protected override void OnStart()
    {
        base.OnStart();
        MainGameUIController.I.endingManager.SetEnding(endingName, true);
    }
}
