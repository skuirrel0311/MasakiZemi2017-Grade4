using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPageGuide : GuideArrow
{
    [SerializeField]
    HoloText text = null;

    [SerializeField]
    HoloText p2Attention = null;

    public override void ShowGuideArrow()
    {
        if(MainSceneManager.I.currentPageIndex == 1)
        {
            p2Attention.gameObject.SetActive(true);
        }
        text.gameObject.SetActive(true);
        base.ShowGuideArrow();
    }

    public override void HideGuideArrow()
    {
        if (MainSceneManager.I.currentPageIndex == 1)
        {
            p2Attention.gameObject.SetActive(false);
        }
        text.gameObject.SetActive(false);
        base.HideGuideArrow();
    }

}
