using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScale : BaseStateMachineBehaviour
{
    [SerializeField]
    string objName = "";

    [SerializeField]
    float scaleRate = 1.0f;

    HoloObject obj;

    protected override void OnStart()
    {
        base.OnStart();

        obj = ActorManager.I.GetObject(objName);

        StateMachineManager.I.StartCoroutine(ScaleChange());
    }

    IEnumerator ScaleChange()
    {
        obj.gameObject.SetActive(false);
        yield return null;

        obj.ChangeScale(scaleRate);
        yield return null;

        obj.gameObject.SetActive(true);
    }
}
