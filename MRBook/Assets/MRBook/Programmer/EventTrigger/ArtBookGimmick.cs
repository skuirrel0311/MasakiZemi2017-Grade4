using UnityEngine;

/// <summary>
/// ギミックで実際に動くモデルにつけるスクリプト
/// flagNameには隠れているか？を入れる
/// </summary>
public class ArtBookGimmick : MyEventTrigger
{
    protected bool isHide = false;

    //面法線
    protected Vector3 normalVec;

    [SerializeField]
    protected GimmickMaker maker = null;

    //本来ある場所
    [SerializeField]
    protected IsRendered original = null;
    
    protected Renderer m_renderer;
    protected Collider m_collider;

    protected virtual void Start()
    {
        normalVec = transform.up;
        m_renderer = GetComponent<Renderer>();
        m_collider = GetComponent<Collider>();

        if (maker == null) return;

        maker.foundMakerEvent = () =>
        {
            SetGimmickVisuable(true);
            isHide = false;
        };

        maker.lostMakerEvent = () =>
        {
            //見えているはず
            if (original.WasRendered)
            {
                //隠れた
                isHide = true;
                SetGimmickVisuable(false);
            }
            else
            {
                //普通に見えなくなっただけ
                isHide = false;
            }
        };
    }

    protected virtual void Update()
    {
    }
    
    public override void SetFlag()
    {
        FlagManager.I.SetFlag(flagName,this, isHide);
    }

    protected void SetGimmickVisuable(bool isVisuable)
    {
        m_renderer.enabled = isVisuable;
        m_collider.enabled = isVisuable;
    }
}
