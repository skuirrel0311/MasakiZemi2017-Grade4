using System;
using UnityEngine;

/// <summary>
/// ギミックをに認識するマーカーにつけるスクリプト
/// </summary>
public class GimmickMaker : MyTracableEventHandler
{
    /// <summary>
    /// 見えているか？(マーカーを認識できているか？)
    /// </summary>
    public bool IsVisuable { get; private set; }

    /// <summary>
    /// 見えていたことがあるか？(初回を弾く用)
    /// </summary>
    public bool IsVisuabled { get; private set; }

    public float moveSpeed = 100.0f;

    public Transform child { get; private set; }

    Vector3 currentPosition;
    Vector3 oldPosition;

    bool isHide = true;

    public Action foundMakerEvent;
    public Action lostMakerEvent;

    protected override void Start()
    {
        base.Start();

        child = transform.GetChild(0);
        //親子関係を切る
        child.parent = null;
    }

    void Update()
    {
        if (isHide) return;

        if (!IsVisuable)
        {
            float temp = Vector3.Distance(child.position, oldPosition);
            if (temp < 0.05f)
            {
                isHide = true;
            }
        }
        else
        {
            //動きはある程度補間してあげないと気持ち悪い
            currentPosition = transform.position;
        }
        float distance = Vector3.Distance(currentPosition, oldPosition);
        
        child.position = Vector3.Lerp(oldPosition, currentPosition, (moveSpeed / distance) * Time.deltaTime);
        
        oldPosition = child.position;
    }

    protected override void OnTrackingFound()
    {
        IsVisuable = true;
        IsVisuabled = true;
        isHide = false;
        currentPosition = transform.position;
        oldPosition = currentPosition;

        if (foundMakerEvent != null) foundMakerEvent.Invoke();
    }

    protected override void OnTrackingLost()
    {
        IsVisuable = false;

        if (lostMakerEvent != null) lostMakerEvent.Invoke();
    }
}
