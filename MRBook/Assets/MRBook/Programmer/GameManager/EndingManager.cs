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

    public void Show(string message)
    {
        text.CurrentText = message;
        Show();
    }

    public void Hide()
    {
        text.Color = Color.clear;
        sprite.Color = Color.clear;
    }
}
