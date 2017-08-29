using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HoloToolkit.Unity.InputModule;

[RequireComponent(typeof(BoxCollider))]
public class HoloButton : MyInputHandler
{
    public enum HoloButtonState { Normal, Over, Pressed, Disabled }

    [SerializeField]
    UnityEvent onClick = null;

    [SerializeField]
    bool autoHide = false;

    [SerializeField]
    Color pressedColor = Color.gray;
    [SerializeField]
    Color disabledColor = new Color(0.2f, 0.2f, 0.2f);
    Color clearColor = Color.clear;
    
    //あとで消える予定
    [SerializeField]
    HoloText text = null;
    
    List<IHoloUI> imageList = new List<IHoloUI>();

    BoxCollider m_collider;

    Coroutine changeColorCoroutine;
    public bool isChangeColor = false;

    protected void Awake()
    {
        base.Start();

        m_collider = GetComponent<BoxCollider>();
        imageList.AddRange(GetComponentsInChildren<HoloSprite>());
    }

    protected override void StartDragging()
    {
        base.StartDragging();
        ChangeButtonColor(pressedColor);
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

        Debug.LogError("on click");
        onClick.Invoke();

        if (autoHide) Disable();
        else Refresh();
    }
    
    void ChangeButtonColor(Color targetColor, float duration = 0.1f)
    {
        if (imageList.Count <= 0) return;

        Color start = imageList[0].Color;

        if (start.Equals(targetColor)) return;

        if(isChangeColor)
        {
            StopCoroutine(changeColorCoroutine);
        }

        isChangeColor = true;
        Color currentColor;
        changeColorCoroutine = StartCoroutine(KKUtilities.FloatLerp(duration, t =>
        {
            currentColor = Color.Lerp(start, targetColor, t * t);
            for (int i = 0; i < imageList.Count; i++)
            {
                imageList[i].Color = currentColor;
            }
        }).OnCompleted(() => isChangeColor = false));

    }

    public void Refresh()
    {
        text.gameObject.SetActive(true);
        m_collider.enabled = true;
        ChangeButtonColor(Color.white);
    }

    //見えるが押せない状態
    public void Disable()
    {
        m_collider.enabled = false;
        ChangeButtonColor(disabledColor);
    }

    public void Hide()
    {
        m_collider.enabled = false;
        text.gameObject.SetActive(false);
        ChangeButtonColor(clearColor);
    }
}
