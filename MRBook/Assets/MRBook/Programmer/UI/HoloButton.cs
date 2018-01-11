using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HoloToolkit.Unity.InputModule;
using KKUtilities;

//スプライトがない場合も考慮しなければならない？？(Alphaを0にすればよいだけ)
[RequireComponent(typeof(BoxCollider))]
public class HoloButton : MyInputHandler
{
    public enum ButtonState { Normal, Over, Pressed, Disabled }
    public enum RefreshState { Refresh, Disable, Hide }

    [SerializeField]
    protected UnityEvent onClick = null;

    [SerializeField]
    protected RefreshState clickedState = RefreshState.Refresh;

    [SerializeField]
    Color pressedColor = Color.gray;
    [SerializeField]
    Color disabledColor = new Color(0.2f, 0.2f, 0.2f);
    Color clearColor = Color.clear;

    [SerializeField]
    HoloText text = null;

    List<IHoloUI> imageList = new List<IHoloUI>();

    BoxCollider m_collider;

    Coroutine changeColorCoroutine;
    bool isChangeColor = false;

    protected void Awake()
    {
        base.Start();

        m_collider = GetComponent<BoxCollider>();
        imageList.AddRange(GetComponentsInChildren<HoloSprite>());
    }

    protected override void StartDragging()
    {
        base.StartDragging();
        ChangeButtonColor(pressedColor, false);
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

        Push();
    }

    void ChangeButtonColor(Color targetColor, bool isInstantly, float duration = 0.1f)
    {
        if (imageList.Count <= 0) return;
        
        if (isChangeColor)
        {
            StopCoroutine(changeColorCoroutine);
        }

        //瞬時に変更する
        if (isInstantly)
        {
            for (int i = 0; i < imageList.Count; i++)
            {
                imageList[i].Color = targetColor;
            }
            isChangeColor = false;
            return;
        }

        Color start = imageList[0].Color;

        if (start.Equals(targetColor)) return;

        isChangeColor = true;
        Color currentColor;
        changeColorCoroutine = StartCoroutine(Utilities.FloatLerp(duration, t =>
        {
            currentColor = Color.Lerp(start, targetColor, t * t);
            for (int i = 0; i < imageList.Count; i++)
            {
                imageList[i].Color = currentColor;
            }
        }).OnCompleted(() => isChangeColor = false));

    }

    public void AddListener(UnityAction action)
    {
        onClick.AddListener(action);
    }

    public void RemoveAllListener()
    {
        onClick.RemoveAllListeners();
    }

    public void Refresh(bool isInstantly = false)
    {
        if (text != null) text.gameObject.SetActive(true);
        m_collider.enabled = true;
        ChangeButtonColor(Color.white, isInstantly);
    }

    //見えるが押せない状態
    public void Disable(bool isInstantly = false)
    {
        m_collider.enabled = false;
        ChangeButtonColor(disabledColor, isInstantly);
    }

    public void Hide(bool isInstantly = false)
    {
        m_collider.enabled = false;
        if (text != null) text.gameObject.SetActive(false);
        ChangeButtonColor(clearColor, isInstantly);
    }

    public void Push()
    {
        if (onClick != null) onClick.Invoke();

        switch (clickedState)
        {
            case RefreshState.Refresh:
                Refresh();
                break;
            case RefreshState.Disable:
                Disable();
                break;
            case RefreshState.Hide:
                Hide();
                break;
        }
    }

    public void SetDefaultButtonState(ButtonState state)
    {
        //デフォルトでOverはないだろう
        switch (state)
        {
            case ButtonState.Normal:
                Refresh(true);
                break;
            case ButtonState.Pressed:
                Disable(true);
                break;
            case ButtonState.Disabled:
                Hide(true);
                break;
        }
    }
}
