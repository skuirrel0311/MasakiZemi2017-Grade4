using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionTestSceneManager : BaseManager<MotionTestSceneManager>
{
    [SerializeField]
    HoloMovableObject actor = null;

    [SerializeField]
    MotionName[] animationNames = null;

    int currentNameIndex = 0;

    [SerializeField]
    HoloItem item = null;

    GameObject targetObj = null;

    protected override void Start()
    {
        base.Start();

        targetObj = GameObject.FindGameObjectWithTag("Target");

        GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Actor");

        for (int i = 0; i < tempArray.Length; i++)
        {
            HoloObject obj = tempArray[i].GetComponent<HoloObject>();

            if (obj.GetActorType == HoloObject.HoloObjectType.Statics) return;

            HoloMovableObject movableObject = (HoloMovableObject)obj;
            movableObject.ApplyDefaultTransform();
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            currentNameIndex++;

            if (currentNameIndex >= animationNames.Length)
            {
                currentNameIndex = 0;
            }

            string motionName = MotionNameManager.GetMotionName(animationNames[currentNameIndex], actor);
            Debug.Log("call change Animation " + motionName);
            actor.m_animator.CrossFade(motionName, 0.1f);
        }
    }

    public void SetItem()
    {
        if (item == null) return;
        if (actor.GetActorType != HoloObject.HoloObjectType.Character) return;

        HoloCharacter character = (HoloCharacter)actor;

        character.SetItem(item.gameObject);

    }

    public void DumpItem()
    {
        if (actor.GetActorType != HoloObject.HoloObjectType.Character) return;

        HoloCharacter character = (HoloCharacter)actor;

        character.DumpItem(HoloItem.Hand.Both);
    }

    public void GoThere()
    {
        Vector3 targetPosition = targetObj.transform.position;
        targetPosition.y = 0.0f;
        actor.m_agent.stoppingDistance = 0.01f;
        actor.m_agent.SetDestination(targetPosition);
    }
}
