using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEventTrigger : MonoBehaviour
{
    [SerializeField]
    protected string flagName = "";

    [SerializeField]
    protected GameObject targetObject = null;

    public virtual void SetFlag() { }
}
