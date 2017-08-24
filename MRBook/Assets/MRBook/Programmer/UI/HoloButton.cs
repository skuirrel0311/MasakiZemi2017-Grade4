using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HoloToolkit.Unity.InputModule;

public class HoloButton : MyInputHandler
{
    enum ButtonState { Normal, Over, Pressed, Disabled }

    [SerializeField]
    UnityEvent onClick = null;

    [SerializeField]
    bool autoHide = false;

    [SerializeField]
    Color normalColor = Color.white;
    [SerializeField]
    Color pressedColor = Color.gray;
    [SerializeField]
    Color disabledColor = new Color(0.2f, 0.2f, 0.2f);
    Color clearColor = Color.clear;

    [SerializeField]
    HoloSprite mainSprite = null;
    [SerializeField]
    HoloSprite limSprite = null;
    [SerializeField]
    HoloText text = null;

    List<IHoloUI> imageList = new List<IHoloUI>();

    ButtonState currentState = ButtonState.Normal;

    protected override void Start()
    {
        base.Start();

        if (text != null) imageList.Add(text);
        if (mainSprite != null) imageList.Add(mainSprite);
    }

    protected override void StartDragging()
    {
        base.StartDragging();
        currentState = ButtonState.Pressed;
        ChangeLimState(ButtonState.Normal);
    }

    protected override void StopDragging()
    {
        base.StopDragging();

        if (!gameObject.Equals(InputManager.Instance.cursor.TargetedObject))
        {
            //クリック判定にはしない
            Refresh();
            return;
        }

        onClick.Invoke();

        if (autoHide) Disable();
        else Refresh();
    }

    public void Refresh()
    {
        ChangeLimState(ButtonState.Disabled);
        ChangeState(currentState, ButtonState.Normal);
    }

    public void Disable()
    {
        ChangeLimState(ButtonState.Disabled);
        ChangeState(currentState, ButtonState.Disabled);
    }

    void ChangeState(ButtonState previousState, ButtonState newState, float duration = 0.1f)
    {
        Color startColor = GetStateColor(previousState);
        Color endColor = GetStateColor(newState);

        StartCoroutine(KKUtilities.FloatLerp(duration, (t) =>
        {
            for (int i = 0; i < imageList.Count; i++)
            {
                imageList[i].Color = Color.Lerp(startColor, endColor, t);
            }
        }));
    }
    Color GetStateColor(ButtonState state)
    {
        switch (state)
        {
            case ButtonState.Normal:
                return normalColor;
            case ButtonState.Pressed:
                return pressedColor;
            case ButtonState.Disabled:
                return disabledColor;
        }

        return Color.white;
    }

    //渡すstateはNormalかDisableのみ
    void ChangeLimState(ButtonState state, float duration = 0.1f)
    {
        if (limSprite == null) return;

        Color startColor = state == ButtonState.Disabled ? normalColor : clearColor;
        Color endColor = state == ButtonState.Normal ? normalColor : clearColor;
        StartCoroutine(KKUtilities.FloatLerp(duration, (t) =>
        {
            limSprite.Color = Color.Lerp(startColor, endColor, t);
        }));
    }
}
