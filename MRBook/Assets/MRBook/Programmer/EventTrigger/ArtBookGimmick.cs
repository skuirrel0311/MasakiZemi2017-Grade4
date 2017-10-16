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

    Camera mainCamera;

    protected Renderer m_renderer;

    protected virtual void Start()
    {
        mainCamera = Camera.main;
        normalVec = transform.up;
        m_renderer = GetComponent<Renderer>();

        if (maker == null) return;

        maker.foundMakerEvent = () =>
        {
            m_renderer.enabled = true;
            isHide = false;
        };

        maker.lostMakerEvent = () =>
        {
            //見えているはず
            if (original.WasRendered)
            {
                //隠れた
                isHide = true;
                m_renderer.enabled = false;
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
}
