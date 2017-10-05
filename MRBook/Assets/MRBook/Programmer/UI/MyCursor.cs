using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using KKUtilities;

public class MyCursor : HoloToolkit.Unity.InputModule.Cursor
{
    enum CursorImageState
    {
        Normal,     //指もオブジェクトも認識していない状態
        Finger,     //指を認識している（オブジェクトは認識していない）
        Hold,       //指が倒れている状態を認識している（オブジェクトは認識していない）
        PosCon,     //PositionControllerを認識している（指は認識していない）
        RotCon,     //RotationControllerを認識している（指は認識していない）
        PosConFin,  //PositionControllerを認識している（指も認識している）
        RotConFin,  //RotationControllerを認識している（指も認識している）
        PosConHol,  //PositionControllerを認識している（指が倒れている状態を認識している）
        RotConHol   //RotationControllerを認識している（指が倒れている状態を認識している）
    }
    CursorImageState imageState = CursorImageState.Normal;

    [SerializeField]
    SpriteRenderer spriteRenderer = null;
    [SerializeField]
    Sprite[] sprites = null;

    bool isRecognizedFinger = false;
    bool isRecognizedPosCon = false;
    bool isRecognizedRotCon = false;
    bool isRecognizedHold = false;

    [SerializeField]
    Transform boundingBox = null;

    Vector3 defaultPosition;
    Quaternion defaultRotation;
    Transform defaultParent;
    Vector3 defaultScale;

    [SerializeField]
    float resetSpeed = 0.2f;

    protected override void Start()
    {
        base.Start();
        
        defaultPosition = PrimaryCursorVisual.localPosition;
        defaultRotation = PrimaryCursorVisual.localRotation;
        defaultScale = PrimaryCursorVisual.localScale;
        defaultParent = PrimaryCursorVisual.parent;
    }

    void OnFlagChanged()
    {
        CursorImageState temp;

        //3 ～ 8 までの処理
        if (isRecognizedRotCon || isRecognizedPosCon)
        {
            temp = isRecognizedPosCon ? CursorImageState.PosCon : CursorImageState.RotCon;

            if (isRecognizedHold)
            {
                temp += 4;
            }
            else if (isRecognizedFinger)
            {
                temp += 2;
            }
            ChangeCursorImage(temp);
            return;
        }

        //0 ～ 2 までの処理
        temp = CursorImageState.Normal;

        if (isRecognizedHold)
        {
            temp = CursorImageState.Hold;
        }
        else if (isRecognizedFinger)
        {
            temp = CursorImageState.Finger;
        }

        ChangeCursorImage(temp);
    }

    //imageStateを渡すのはNG
    void ChangeCursorImage(CursorImageState state)
    {
        if (state == imageState) return;

        imageState = state;
        spriteRenderer.sprite = sprites[(int)state];
    }

    protected override void OnFocusedObjectChanged(GameObject previousObject, GameObject newObject)
    {
        base.OnFocusedObjectChanged(previousObject, newObject);

        if ((isRecognizedPosCon || isRecognizedRotCon) && isRecognizedHold) return;

        if (newObject == null)
        {
            isRecognizedPosCon = false;
            isRecognizedRotCon = false;
            OnFlagChanged();
            return;
        }

        //上の条件分岐とまとめられるが動かせないオブジェクトを見つめていることを検出しているのがこっち
        if (!boundingBox.Equals(newObject.transform.root))
        {
            isRecognizedPosCon = false;
            isRecognizedRotCon = false;
            OnFlagChanged();
            return;
        }

        //BoundingBoxで動かしているオブジェクトかRotationControllerである

        isRecognizedPosCon = boundingBox.Equals(TargetedObject.transform.parent);

        //絶対に反対になる
        isRecognizedRotCon = !isRecognizedPosCon;
        OnFlagChanged();
    }

    public override void OnSourceDetected(SourceStateEventData eventData)
    {
        isRecognizedFinger = true;
        OnFlagChanged();
        base.OnSourceDetected(eventData);
    }
    public override void OnSourceLost(SourceStateEventData eventData)
    {
        isRecognizedFinger = false;
        isRecognizedHold = false;
        OnFlagChanged();
        base.OnSourceLost(eventData);
    }

    public override void OnInputDown(InputEventData eventData)
    {
        if (isRecognizedPosCon || isRecognizedRotCon)
        {
            PrimaryCursorVisual.parent = boundingBox;
            PrimaryCursorVisual.localScale = PrimaryCursorVisual.localScale * (boundingBox.lossyScale.x * 3.0f);
        }

        isRecognizedHold = true;
        OnFlagChanged();
        base.OnInputDown(eventData);
    }
    public override void OnInputUp(InputEventData eventData)
    {
        base.OnInputUp(eventData);
        isRecognizedHold = false;
        //ひとまず
        AkSoundEngine.PostEvent("Tap", gameObject);

        if (!isRecognizedPosCon && !isRecognizedRotCon && TargetedObject != null)
        {
            if (boundingBox.Equals(TargetedObject.transform.parent))
            {
                //バンディングボックスが出現した
                OnFocusedObjectChanged(null, TargetedObject);
                return;
            }
        }

        if (isRecognizedPosCon || isRecognizedRotCon)
        {
            ResetCursorTransform();

            OnFocusedObjectChanged(null, TargetedObject);
            return;
        }

        OnFlagChanged();

    }

    void ResetCursorTransform()
    {
        PrimaryCursorVisual.parent = transform.GetChild(0);
        spriteRenderer.color = Color.clear;
        Vector3 startScale = PrimaryCursorVisual.localScale;

        PrimaryCursorVisual.localPosition = defaultPosition;
        PrimaryCursorVisual.localRotation = defaultRotation;

        StartCoroutine(Utilities.FloatLerp(resetSpeed, (t) =>
        {
            spriteRenderer.color = Color.Lerp(Color.clear, Color.white, t * t);
            PrimaryCursorVisual.localScale = Vector3.Lerp(startScale, defaultScale, t * t);
        }));
    }
}
