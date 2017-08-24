using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HoloToolkit.Unity.InputModule;

public class HoloButton : MyInputHandler
{
    [SerializeField]
    UnityEvent onClick = null;

    [SerializeField]
    bool autoHide = false;

    [SerializeField]
    Color normalColor = Color.white;
    [SerializeField]
    Color pressedColor = Color.gray;
    
    HoloSprite mainSprite = null;
    [SerializeField]
    HoloSprite limSprite = null;

    HoloText text = null;

    List<IHoloUI> imageList = new List<IHoloUI>();

    protected override void Start()
    {
        base.Start();
        
        text = GetComponent<HoloText>();
        mainSprite = GetComponent<HoloSprite>();

        if (text != null) imageList.Add(text);
        if (mainSprite != null) imageList.Add(mainSprite);
        if (limSprite != null) imageList.Add(limSprite);

    }

    protected override void StartDragging()
    {
        base.StartDragging();

    }

    protected override void StopDragging()
    {
        base.StopDragging();
        if(!gameObject.Equals(InputManager.Instance.cursor.TargetedObject))
        {
            //クリック判定にはしない
            return;
        }

        Hide();
        onClick.Invoke();

        if (!autoHide) return;

        KKUtilities.Delay(0.1f, () => Refresh(), this);
    }

    

    public void Refresh()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
