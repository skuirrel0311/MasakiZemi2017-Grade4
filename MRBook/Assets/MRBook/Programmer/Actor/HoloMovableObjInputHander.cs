using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HoloMovableObjInputHander : HoloObjInputHandler
{
    [SerializeField]
    bool isFloating = false;

    public NavMeshAgent m_agent { get; protected set; }

    enum ColType { inPlay, inOperation }
    BoxCollider inPlayCol;
    BoxCollider inOperationCol;

    GameObject arrow;

    void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        BoxCollider[] cols = GetComponents<BoxCollider>();

        if (cols == null) return;

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].isTrigger) continue;

            if (inPlayCol == null)
            {
                inPlayCol = cols[i];
                continue;
            }

            inOperationCol = cols[i];
        }
    }

    void OnEnable()
    {
        if (m_agent != null) m_agent.enabled = false;
    }

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

        MainSceneManager.I.OnPageLoaded += (page) =>
        {
            SetCollision(ColType.inOperation);
        };

        MainSceneManager.I.OnPlayPage += () =>
        {
            SetCollision(ColType.inPlay);
            SetArrowActive(false);
        };

        MainSceneManager.I.OnReset += () =>
        {
            SetCollision(ColType.inOperation);
            SetArrowActive(true);
        };
    }

    void SetCollision(ColType type)
    {
        if (inPlayCol == null || inOperationCol == null) return;
        inPlayCol.enabled = type == ColType.inPlay;
        inOperationCol.enabled = type == ColType.inOperation;
    }

    public virtual void SetArrowActive(bool isActive)
    {
        if (arrow != null) arrow.SetActive(isActive);
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
        SetArrowActive(false);
        base.OnClick();
        return true;
    }

    public override void OnDisabled()
    {
        if (!MyObjControllerByBoundingBox.I.canClick) return;
        SetArrowActive(true);
        base.OnDisabled();
    }
}
