using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BubleItemInputHandler : ItemInputHandler
{
    [SerializeField]
    NavMeshAgent agent = null;

    GameObject bubble;

    public override void Init(HoloObject owner)
    {
        base.Init(owner);

        m_agent = agent;

        bubble = transform.GetChild(0).gameObject;
    }

    public override bool OnClick()
    {
        //泡がはじけて下に落ちる
        StartCoroutine(Fall());
        bubble.SetActive(false);
        AkSoundEngine.PostEvent("Bubble", owner.gameObject);

        return base.OnClick();
    }

    IEnumerator Fall()
    {
        NavMeshHit hit;
        while (true)
        {
            //0.1ずつ下を探す
            MyNavMeshBuilder.CreateNavMesh();
            owner.transform.position += Vector3.down * 0.03f;

            //todo : 絵本よりも下に行った場合はやばいのでなにか対応が必要

            if (NavMesh.SamplePosition(owner.transform.position, out hit, m_agent.height, NavMesh.AllAreas))
            {
                Debug.Log("found sample position");
                break;
            }

            yield return null;
        }

        m_agent.enabled = true;

        yield return null;


        MyObjControllerByBoundingBox.I.Disable();
        OnDisabled();
        m_agent.enabled = false;
    }
}
