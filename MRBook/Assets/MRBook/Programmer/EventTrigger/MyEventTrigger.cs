using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEventTrigger : MonoBehaviour
{
    [SerializeField]
    protected string flagName = "";

    [SerializeField]
    protected GameObject targetObject = null;
    [SerializeField]
    protected LayerMask layer;

    public virtual void SetFlag() { }
}
