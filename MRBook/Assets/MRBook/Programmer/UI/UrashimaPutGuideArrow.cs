using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrashimaPutGuideArrow : GuideArrow
{
    protected override void Start()
    {
        MainSceneManager.I.OnPlayPage += OnPlay;

        base.Start();
    }

    void OnPlay()
    {
        emitter.gameObject.SetActive(false);
        StopCoroutine(moveEmitterCoroutine);
        MainSceneManager.I.OnPlayPage -= OnPlay;
    }
}
