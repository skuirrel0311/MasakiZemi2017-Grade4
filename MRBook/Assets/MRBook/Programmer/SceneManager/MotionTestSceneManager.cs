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

    Dictionary<string, HoloCharacter> actorDictionary = new Dictionary<string, HoloCharacter>();

    HoloObjResetManager resetManager;

    protected override void Start()
    {
        base.Start();

        resetManager = new HoloObjResetManager(this);

        targetObj = GameObject.FindGameObjectWithTag("Target");

        GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Actor");

        for (int i = 0; i < tempArray.Length; i++)
        {
            HoloObject obj = null;
            try
            {
                obj = tempArray[i].GetComponent<HoloObject>();
            }
            catch
            {
                Debug.Log(tempArray[i].name + " is not actor");
            }
            if (obj == null) continue;
            if (obj.GetActorType != HoloObject.Type.Character) continue;

            HoloCharacter character = (HoloCharacter)obj;
            character.InitItemSaucer();
            Debug.Log("add " + character.name);
            actorDictionary.Add(character.name, character);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentNameIndex++;

            if (currentNameIndex >= elements.Length)
            {
                currentNameIndex = 0;
            }

            character = GetActor(actorName);
            string motionName = MotionNameManager.GetMotionName(elements[currentNameIndex].name, (CharacterItemSaucer)character.ItemSaucer);
            stateText.text = motionName;
            Debug.Log("call change Animation " + motionName);
            character.m_animator.CrossFade(motionName, elements[currentNameIndex].transitionTime);
        }
    }

    public void SetItem()
    {
        character = GetActor(actorName);

        character.ItemSaucer.SetItem(item);
    }

    public void DumpItem()
    {
        character = GetActor(actorName);
        character.ItemSaucer.DumpItem();
    }

    public void GoThere()
    {
        Vector3 targetPosition = targetObj.transform.position;
        targetPosition.y = 0.0f;
        character = GetActor(actorName);
        character.m_agent.stoppingDistance = 0.01f;
        character.m_agent.SetDestination(targetPosition);
    }

    HoloCharacter GetActor(ActorName actorName)
    {
        return actorDictionary[actorName.ToString()];
    }
}
