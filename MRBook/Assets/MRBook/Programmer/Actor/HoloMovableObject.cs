using UnityEngine;
using UnityEngine.AI;
using HoloToolkit.Unity.InputModule;

/// <summary>
/// 動かすことのできるホログラム
/// </summary>
[RequireComponent(typeof(BoxCollider))]  //BoundingBoxの形状を決めるために必須(トリガーも可)
public class HoloMovableObject : HoloObject, IInputClickHandler
{
    public override Type GetActorType { get { return Type.Movable; } }

    /*
     * M = true  B = false  //元のページでのみ動かせる
     * M = false B = true   //元のページでは動かせないが別のページに持っていくことができる
     * M = true  B = true   //元のページで動かせるし別のページに持っていくこともできる
     * の３パターンある
     */
    //public bool isMovable = false;  //元のページで動かせるか？
    public bool isBring = false;    //別のページに持っていくことができるか？

    public virtual bool IsGrounding { get { return false; } }
    
    //初期値（リセットボタンを押した時に戻すための値）
    protected Vector3 firstPosition;
    protected Quaternion firstRotation;

    //BoundingBoxの形状を決める
    public BoxCollider m_collider { get; protected set; }
    public float SphereCastRadius { get; private set; }

    //動かせることを明示するための矢印
    protected GameObject triangle;

    /// <summary>
    /// 再生中にセットするLayer
    /// </summary>
    [SerializeField]
    int inPlayLayer = 4;
    protected int defaultLayer;

    protected virtual void Awake()
    {
        m_collider = GetComponent<BoxCollider>();

        float colSize = Mathf.Max(m_collider.size.x, m_collider.size.z);
        float scale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
        SphereCastRadius = colSize * scale;
        defaultLayer = gameObject.layer;
    }

    /// <summary>
    /// ページが開かれた
    /// </summary>
    /// <param name="isFirst">そのページを開くのが初めてか？</param
    public override void PageStart(int currentPageIndex)
    {
        if (isFirst)
        {
            ApplyDefaultTransform();
        }

        if (isMovable)
        {
            ActivateControl();
        }

        base.PageStart(currentPageIndex);
    }

    public override void PlayPage()
    {
        if(triangle != null) triangle.SetActive(false);
        gameObject.layer = inPlayLayer;
    }

    /// <summary>
    /// ページが初めて開かれた時の場所に戻す
    /// </summary>
    public override void ResetTransform()
    {
        gameObject.layer = defaultLayer;
        transform.position = firstPosition;
        transform.rotation = firstRotation;
        
        if(isMovable)
        {
            triangle.SetActive(true);
        }

        //ほかのページに持っていけるオブジェクトの場合はグローバルになっている可能性がある
        if (isBring)
        {
            ActorManager.I.RemoveGlobal(gameObject.name);
        }

        base.ResetTransform();
    }

    /// <summary>
    /// 現在の座標と角度をデフォルトとして登録する
    /// </summary>
    public void ApplyDefaultTransform()
    {
        firstPosition = transform.position;
        firstRotation = transform.rotation;
    }

    /// <summary>
    /// デフォルトの位置をずらす
    /// </summary>
    public void ApplyDefaultTransform(Vector3 movement)
    {
        firstPosition += movement;
    }

    void ActivateControl()
    {
        if(triangle != null)
        {
            triangle.SetActive(true);
            return;
        }
        //三角形の位置を決める
        triangle = Instantiate(ActorManager.I.trianglePrefab, transform);
        triangle.transform.localPosition = Vector3.up * m_collider.size.y * 1.0f;
        float scale = m_collider.size.x * transform.lossyScale.x * 0.5f;
        scale = Mathf.Clamp(scale, 0.02f, 0.08f);
        triangle.transform.localScale = Vector3.one * scale * (1.0f / transform.lossyScale.x);
        Debug.Log(gameObject.name + "は操作可能だ");
    }

    public virtual void Fall() { }

    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        if (!isMovable) return;

        MyObjControllerByBoundingBox.I.SetTargetObject(gameObject);
    }

    public override bool Equals(object other)
    {
        return gameObject.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
