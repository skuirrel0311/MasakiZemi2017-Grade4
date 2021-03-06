﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HoloMovableObjInputHander : HoloObjInputHandler
{
    [SerializeField]
    bool isFloating = false;

    GameObject arrow;

    public override void Init(HoloObject owner)
    {
        base.Init(owner);
        if (isFloating) AddBehaviour(new FloatingObjDragEndBehaviour(owner));
        else AddBehaviour(new GroundingObjDragEndBehaviour(owner));

        //矢印
        if (MainSceneManager.I == null) return;
        arrow = Instantiate(ActorManager.I.trianglePrefab, transform);
        arrow.transform.localPosition = Vector3.up * m_collider.size.y * 1.0f;
        float scale = m_collider.size.x * transform.lossyScale.x * 0.5f;
        scale = Mathf.Clamp(scale, 0.02f, 0.08f);
        arrow.transform.localScale = Vector3.one * scale * (1.0f / transform.lossyScale.x);
        MainSceneManager.I.OnPlayPage += () =>
        {
            arrow.SetActive(false);
        };

        MainSceneManager.I.OnReset += () =>
        {
            arrow.SetActive(true);
        };
    }

    protected override void SetSphreCastRadius()
    {
        if (!isFloating)
        {
            //NavMeshAgentでRadiusを決める
            NavMeshAgent agent = owner.GetComponent<NavMeshAgent>();

            if (agent != null)
            {
                SphereCastRadius = agent.radius * owner.transform.lossyScale.x;
                return;
            }
        }

        base.SetSphreCastRadius();
    }

    public override bool OnClick()
    {
        if (!MyObjControllerByBoundingBox.I.canClick) return false;
        MyObjControllerByBoundingBox.I.SetTargetObject(owner);
        if (arrow != null) arrow.SetActive(false);
        base.OnClick();
        return true;
    }

    public override void OnDisabled()
    {
        if(arrow != null)arrow.SetActive(true);
        base.OnDisabled();
    }
}
