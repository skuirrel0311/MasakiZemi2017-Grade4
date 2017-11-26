using UnityEngine;
using UnityEngine.AI;
using HoloToolkit.Unity.InputModule;

/// <summary>
/// 動く可能性のあるオブジェクト（位置のリセットが必要だということ）
/// </summary>
[HideInInspector]
public class HoloMovableObject : HoloObject, IInputClickHandler
{
    /*
     * M = true  B = false  //元のページでのみ動かせる
     * M = false B = true   //元のページでは動かせないが別のページに持っていくことができる
     * M = true  B = true   //元のページで動かせるし別のページに持っていくこともできる
     * の３パターンある
     */
    //public bool isMovable = false;  //元のページで動かせるか？
    //public bool isBring = false;    //別のページに持っていくことができるか？

    //public virtual bool IsGrounding { get { return false; } }

    //初期値（リセットボタンを押した時に戻すための値）
    //protected Vector3 firstPosition;
    //protected Quaternion firstRotation;
    
    //BoundingBoxの形状を決めるために使用される(トリガーも可)
    public BoxCollider m_collider { get; private set; }
    public NavMeshAgent m_agent { get; private set; }
    //public float SphereCastRadius { get; private set; }

    //動かせることを明示するための矢印
    //public GameObject triangle { get; private set; }

    /// <summary>
    /// 再生中にセットするLayer
    /// </summary>
    [SerializeField]
    int inPlayLayer = 4;
    //protected int defaultLayer;

    protected virtual void Awake()
    {
        if(IsMovable) m_collider = GetComponent<BoxCollider>();
        if (!IsFloating) m_agent = GetComponent<NavMeshAgent>();
        //float colSize = Mathf.Max(m_collider.size.x, m_collider.size.z);
        //float scale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
        //SphereCastRadius = colSize * scale;
        //defaultLayer = gameObject.layer;
    }

    public override void PlayPage()
    {
        //if(triangle != null) triangle.SetActive(false);
        gameObject.layer = inPlayLayer;
    }

    //void ActivateControl()
    //{
    //    if (triangle != null)
    //    {
    //        triangle.SetActive(true);
    //        return;
    //    }
    //    //三角形の位置を決める
    //    triangle = Instantiate(ActorManager.I.trianglePrefab, transform);
    //    triangle.transform.localPosition = Vector3.up * m_collider.size.y * 1.0f;
    //    float scale = m_collider.size.x * transform.lossyScale.x * 0.5f;
    //    scale = Mathf.Clamp(scale, 0.02f, 0.08f);
    //    triangle.transform.localScale = Vector3.one * scale * (1.0f / transform.lossyScale.x);
    //    Debug.Log(gameObject.name + "は操作可能だ");
    //}

    public virtual void OnInputClicked(InputClickedEventData eventData)
    {

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
