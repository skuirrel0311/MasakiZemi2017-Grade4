using UnityEngine;
using UnityEngine.AI;
using HoloToolkit.Unity.InputModule;

/// <summary>
/// 動かすことのできるホログラム
/// </summary>
[RequireComponent(typeof(BoxCollider))]  //BoundingBoxの形状を決めるために必須
[RequireComponent(typeof(NavMeshAgent))] //下に落とすために必須
public class HoloMovableObject : HoloObject, IInputClickHandler
{
    public override HoloObjectType GetActorType { get { return HoloObjectType.Movable; } }

    /*
     * 動かすことができないが別のページに持っていける場合もある。
     * （戻ってきた時のみに動かすことが出来る）
     * 動かせるが別のページに持っていけない場合もある。
     */

    /// <summary>
    /// 動かせるか
    /// </summary>
    public bool isMovable = false;
    /// <summary>
    /// 別のページに持っていけるか
    /// </summary>
    public bool isBring = false;

    /// <summary>
    /// そのオブジェクトが存在する（元の）ページのインデックス
    /// </summary>
    public int pageIndex { get; private set; }

    //初期値
    public Vector3 firstPosition { get; private set; }
    public Quaternion firstRotation { get; private set; }

    public NavMeshAgent m_agent { get; protected set; }
    public BoxCollider m_collider { get; protected set; }
    public Animator m_animator { get; protected set; }

    protected virtual void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_collider = GetComponent<BoxCollider>();
        m_animator = GetComponent<Animator>();
    }

    /// <summary>
    /// ページが開かれた
    /// </summary>
    /// <param name="isFirst">そのページを開くのが初めてか？</param
    public override void PageStart(int currentPageIndex, bool isFirst = true)
    {
        if (isFirst)
        {
            pageIndex = currentPageIndex;
            ApplyDefaultTransform();
        }

        if (isMovable)
        {
            ActivateControl();
        }
        else
        {
            SetGrayScaleShader();
        }
    }

    /// <summary>
    /// ページが初めて開かれた時の場所に戻す
    /// </summary>
    public override void ResetTransform()
    {
        transform.position = firstPosition;
        transform.rotation = firstRotation;

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

    void ActivateControl()
    {
        //操作できるようにする
        Debug.Log(gameObject.name + "は操作可能だ");
    }

    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        if (!isMovable) return;

        MyObjControllerByBoundingBox.I.SetTargetObject(gameObject);
    }
}
