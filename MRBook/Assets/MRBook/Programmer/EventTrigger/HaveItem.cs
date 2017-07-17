using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveItem : MyEventTrigger
{
    bool haveItem = false;
    Actor owner;

    /// <summary>
    /// アイテムを持つ腕(持った時のTransform)
    /// </summary>
    [SerializeField]
    Transform arm = null;

    void Start()
    {
        owner = GetComponent<Actor>();
        if(owner == null)
        {
            Debug.LogError("not found parent actor");
        }
    }

    public override void SetFlag()
    {

    }

    /// <summary>
    /// アイテムを持たせる
    /// </summary>
    public void ToHaveItem(Actor actor)
    {
        //todo:もうすでに持っている場合の処理も考える

        actor.transform.parent = arm;
        actor.transform.position = Vector3.zero;
        actor.transform.rotation = Quaternion.identity;
    }
}
