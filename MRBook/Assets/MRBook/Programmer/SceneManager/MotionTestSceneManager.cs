using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotionTestSceneManager : BaseManager<MotionTestSceneManager>
{
    [SerializeField]
    ActorName actorName;

    HoloMovableObject actor;
    [SerializeField]
    Element[] elements = null;

    [System.Serializable]
    class Element
    {
        public MotionName name;
        public float transitionTime = 0.1f;
    }

    [SerializeField]
    Text stateText = null;

    int currentNameIndex = -1;

    [SerializeField]
    HoloItem item = null;

    GameObject targetObj = null;

    Dictionary<string, HoloMovableObject> actorDictionary = new Dictionary<string, HoloMovableObject>();

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
            actorDictionary.Add(movableObject.name, movableObject);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            currentNameIndex++;

            if (currentNameIndex >= elements.Length)
            {
                currentNameIndex = 0;
            }

            actor = GetActor(actorName);
            string motionName = MotionNameManager.GetMotionName(elements[currentNameIndex].name, actor);
            stateText.text = motionName;
            Debug.Log("call change Animation " + motionName);
            actor.m_animator.CrossFade(motionName, elements[currentNameIndex].transitionTime);
        }
    }

    public void SetItem()
    {
        if (item == null) return;
        actor = GetActor(actorName);

        if (actor.GetActorType != HoloObject.HoloObjectType.Character) return;

        HoloCharacter character = (HoloCharacter)actor;

        character.SetItem(item.gameObject);

    }

    public void DumpItem()
    {
        actor = GetActor(actorName);
        if (actor.GetActorType != HoloObject.HoloObjectType.Character) return;

        HoloCharacter character = (HoloCharacter)actor;

        character.DumpItem(HoloItem.Hand.Both);
    }

    public void GoThere()
    {
        Vector3 targetPosition = targetObj.transform.position;
        targetPosition.y = 0.0f;
        actor = GetActor(actorName);
        actor.m_agent.stoppingDistance = 0.01f;
        actor.m_agent.SetDestination(targetPosition);
    }

    HoloMovableObject GetActor(ActorName actorName)
    {
        return actorDictionary[actorName.ToString()];
    }
}
