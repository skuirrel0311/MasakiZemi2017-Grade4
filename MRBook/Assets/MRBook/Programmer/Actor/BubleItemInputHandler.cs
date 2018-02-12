﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BubleItemInputHandler : ItemInputHandler
{
    [SerializeField]
    NavMeshAgent agent = null;

    [SerializeField]
    GameObject bubble = null;

    bool isPoped = false;

    public override void Init(HoloObject owner)
    {
        Debug.Log("init");
        base.Init(owner);

        m_agent = agent;

        if(bubble == null) bubble = transform.GetChild(0).gameObject;
    }

    public override bool OnClick()
    {
        if (!MyObjControllerByBoundingBox.I.canClick) return false;
        if (isPoped) return base.OnClick();

        isPoped = true;
        
        if(MainSceneManager.I != null) MainSceneManager.I.OnReset += OnReset;

        //泡がはじけて下に落ちる
        StartCoroutine(Fall());
        bubble.SetActive(false);
        AkSoundEngine.PostEvent("Bubble", owner.gameObject);

        return base.OnClick();
    }

    void OnReset()
    {
        isPoped = false;
        bubble.SetActive(true);
        MainSceneManager.I.OnReset -= OnReset;
    }

    IEnumerator Fall()
    {
        NavMeshHit hit;

        float bookHeight = OffsetController.I.bookTransform.position.y;
        float airHeight = bookHeight + 0.5f;
        //Debug.Log("book height = " + bookHeight);

        while (true)
        {
            //0.1ずつ下を探す
            MyNavMeshBuilder.CreateNavMesh();
            owner.transform.position += Vector3.down * 0.03f;

            //todo : 絵本よりも下に行った場合はやばいのでなにか対応が必要

            if (NavMesh.SamplePosition(owner.transform.position, out hit, m_agent.height, NavMesh.AllAreas))
            {
                //Debug.Log("found sample position");
                break;
            }

            if (owner.transform.position.y < bookHeight)
            {
                Vector3 airPosition = owner.transform.position;
                airPosition.y = airHeight;
                owner.transform.position = airPosition;

                yield break;
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
