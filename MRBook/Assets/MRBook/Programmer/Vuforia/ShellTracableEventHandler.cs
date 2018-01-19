using System;
using System.Collections.Generic;
using UnityEngine;

public class ShellTracableEventHandler : MyTracableEventHandler
{
    public Action onFound;

    protected override void OnTrackingFound()
    {
        if (onFound != null) onFound.Invoke();
    }

    protected override void OnTrackingLost() { }
}
