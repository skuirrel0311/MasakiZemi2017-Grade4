using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEventTrigger : MonoBehaviour
{
    [SerializeField]
    protected string flagName = "";

    public virtual void SetFlag() { }
}
