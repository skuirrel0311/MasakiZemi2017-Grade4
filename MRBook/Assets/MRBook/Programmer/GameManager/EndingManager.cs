using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

/// <summary>
/// 状況から死因を推測してくれる人
/// </summary>
public class EndingManager : MonoBehaviour
{
    [SerializeField]
    HoloText text = null;
    [SerializeField]
    HoloSprite sprite = null;
    //固定されたか？
    bool isFixation = false;

    public void Show()
    {
        Color tempColor;
        StartCoroutine(Utilities.FloatLerp(0.1f, (t)=>
        {
            tempColor = Color.Lerp(Color.clear, Color.white, t);
            text.Color = tempColor;
            sprite.Color = tempColor;
        }));
    }

    public void SetEnding(string message, bool isOverride = false)
    {
        if (isFixation && !isOverride) return;
        isFixation = isOverride;

        if (string.IsNullOrEmpty(message)) text.CurrentText = "";
        else text.CurrentText = "死因：" +  message;
    }

    public void Hide()
    {
        isFixation = false;
        text.Color = Color.clear;
        sprite.Color = Color.clear;
    }
}
