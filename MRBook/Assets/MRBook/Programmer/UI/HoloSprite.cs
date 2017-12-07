using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HoloSprite : MonoBehaviour ,IHoloUI
{
    [SerializeField]
    SpriteRenderer spriteRenderer = null;

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
    
    public Color Color
    {
        get
        {
            return spriteRenderer.color;
        }
        set
        {
            spriteRenderer.color = value;
        }
    }
}

public interface IHoloUI
{
    Color Color { get; set; }
}
