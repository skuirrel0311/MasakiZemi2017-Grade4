using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotionTestSceneManager : BaseManager<MotionTestSceneManager>
{
    [SerializeField]
    ActorName actorName = ActorName.Urashima;

    HoloCharacter character;
    [SerializeField]
    Element[] elements = null;

    [System.Serializable]
    class Element
    {
        public MotionName name = MotionName.Wait;
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

            if (obj.GetActorType == HoloObject.Type.Statics) return;

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

            character = (HoloCharacter)(GetActor(actorName));
            string motionName = MotionNameManager.GetMotionName(elements[currentNameIndex].name, character);
            stateText.text = motionName;
            Debug.Log("call change Animation " + motionName);
            character.m_animator.CrossFade(motionName, elements[currentNameIndex].transitionTime);
        }
    }

    public void SetItem()
    {
        character = (HoloCharacter)GetActor(actorName);

        character.SetItem(item.gameObject);
    }

    public void DumpItem()
    {
        character = (HoloCharacter)GetActor(actorName);
        character.DumpItem(HoloItem.Hand.Both);
    }

    public void GoThere()
    {
        Vector3 targetPosition = targetObj.transform.position;
        targetPosition.y = 0.0f;
        character = (HoloCharacter)GetActor(actorName);
        character.m_agent.stoppingDistance = 0.01f;
        character.m_agent.SetDestination(targetPosition);
    }

    HoloMovableObject GetActor(ActorName actorName)
    {
        return actorDictionary[actorName.ToString()];
    }
}
