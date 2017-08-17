using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEventTrigger : MonoBehaviour
{
    public string flagName = "";

    [SerializeField]
    protected GameObject targetObject = null;

    public virtual void SetFlag() { }
}
