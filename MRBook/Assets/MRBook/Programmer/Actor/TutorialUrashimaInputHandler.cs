﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TutorialUrashimaInputHandler : ItemInputHandler
{
    NavMeshAgent agent = null;

    [SerializeField]
    GameObject bubble = null;
    //泡がはじけているか？
    bool isPoped = false;

    HoloCharacter urashima;

    public override void Init(HoloObject owner)
    {
        base.Init(owner);

        urashima = (HoloCharacter)owner;
        m_agent = GetComponent<NavMeshAgent>();
    }

    public override bool OnClick()
    {
        if (!HoloObjectController.I.canClick) return false;

        //もうはじけている場合は普通のやつ
        if (isPoped) return base.OnClick();

        isPoped = true;

        //泡がはじけて下に落ちる
        StartCoroutine(Fall());
        bubble.SetActive(false);
        AkSoundEngine.PostEvent("Bubble", owner.gameObject);

        return true;
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


        HoloObjectController.I.Disable();
        OnDisabled();
        m_agent.enabled = false;
    }

    public override MakerType OnDragUpdate(HitObjType hitObjType, HoloObject hitObj)
    {
        if (hitObj == null || hitObj.ItemSaucer == null)
        {
            return base.OnDragUpdate(hitObjType, hitObj);
        }

        if (hitObjType == HitObjType.EventArea)
        {
            EventAreaItemSaucer eventArea = (EventAreaItemSaucer)hitObj.ItemSaucer;

            if (eventArea.CheckCanHaveItem(urashima)) return MakerType.Happen;
        }

        return MakerType.DontPut;
    }

    public override void OnDragEnd(HitObjType hitObjType, HoloObject hitObj)
    {
        if (hitObjType != HitObjType.EventArea)
        {
            base.OnDragEnd(hitObjType, hitObj);
            return;
        }

        EventAreaItemSaucer eventArea = (EventAreaItemSaucer)hitObj.ItemSaucer;

        if (eventArea.CheckCanHaveItem(urashima))
        {
            eventArea.SetCharacter(urashima, false);
        }
    }
}
