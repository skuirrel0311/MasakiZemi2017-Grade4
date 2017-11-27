using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class BaseObjInputHandler : MonoBehaviour, IInputClickHandler
{
    protected HoloObject owner;
    //BoundingBoxの形状を決めるために使用される(トリガーも可)
    public BoxCollider m_collider { get; private set; }
    public float SphereCastRadius { get; private set; }
    [SerializeField]
    bool isFloating = false;

    public virtual void Init(HoloObject owner)
    {
        this.owner = owner;
        m_collider = GetComponent<BoxCollider>();

        SetSphreCastRadius();
    }

    public enum HitObjType { None, Book, Character, OtherObj }
    public enum MakerType { None, Normal, DontPut, PresentItem, DontPresentItem }
    public virtual void OnClick() { }
    public virtual void OnDragStart() { }
    public virtual MakerType OnDragUpdate(HitObjType hitObjType, HoloObject hitObj) { return MakerType.None; }
    public virtual void OnDragEnd(HitObjType hitObjType, HoloObject hitObj) { }

    protected virtual void SetSphreCastRadius()
    {
        float colSize = Mathf.Max(m_collider.size.x, m_collider.size.z);
        float scale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
        SphereCastRadius = colSize * scale;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        OnClick();
    }
}
