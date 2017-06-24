using UnityEngine;

/// <summary>
/// ギミックで実際に動くモデルにつけるスクリプト
/// flagNameには隠れているか？を入れる
/// </summary>
public class ArtBookGimmick : MyEventTrigger
{
    /// <summary>
    /// 描画されたか？
    /// </summary>
    bool isRendered = false;
    /// <summary>
    /// 角度的には見えているか？
    /// </summary>
    protected bool isInAngle = false;

    /// <summary>
    /// 見えていないといけない(これがtrueになっているのに見えないってことは隠れているということ)
    /// </summary>
    protected bool isMustVisuable = false;

    protected bool isHide = true;

    //面法線
    Vector3 normalVec;

    //見える角度
    float field = 60.0f;

    [SerializeField]
    protected GimmickMaker maker;

    Camera mainCamera;

    protected virtual void Start()
    {
        mainCamera = Camera.main;
        normalVec = transform.up;
    }

    protected virtual void Update()
    {
        float angle = 0.0f;

        Vector3 temp = Vector3.Normalize(mainCamera.transform.forward) * -1.0f;

        angle = Vector3.Angle(normalVec, temp);
        isInAngle = angle < field;

        isMustVisuable = isInAngle && isRendered;

        //見えていなけらばならないのに見えていなかったら隠れている
        isHide = (isMustVisuable && !maker.IsVisuable);

        isRendered = false;
    }

    void OnWillRenderObject()
    {
        if (Camera.current.tag == mainCamera.tag)
        {
            isRendered = true;
        }
    }

    public override void SetFlag()
    {
        FlagManager.I.SetFlag(flagName, isHide);
    }
}
