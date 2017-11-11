using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveChildTask : BaseStateMachineBehaviour
{
    public override bool HasChild { get { return true; } }
    
    protected List<BaseStateMachineBehaviour> childTask = new List<BaseStateMachineBehaviour>();
}
