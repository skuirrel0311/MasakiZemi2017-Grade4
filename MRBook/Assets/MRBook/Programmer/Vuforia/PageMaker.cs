using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageMaker : HoloTracableEventHandler
{
    protected override void OnTrackingFound()
    {
        //ページに遷移するべきか？
        
    }

    //見失った時にすることはない
    protected override void OnTrackingLost() { }
}
