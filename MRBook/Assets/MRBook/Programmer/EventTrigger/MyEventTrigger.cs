using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyEventTrigger : MonoBehaviour
{
    public string flagName = "";

    public abstract void SetFlag();
}
