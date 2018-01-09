using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class SpriteAnimation : MonoBehaviour
{
    public bool IsPlaying { get; private set; }

    GameObject childObject;

    public void Init()
    {
        Transform child = transform.GetChild(0);
        if(child == null)
        {
            Debug.LogError("not found child objced in " + name);
            return;
        }
        childObject = child.gameObject;
    }

    public void Play(float lifeTime)
    {
        IsPlaying = true;
        childObject.SetActive(true);

        Utilities.Delay(lifeTime, () =>
        {
            IsPlaying = false;
            childObject.SetActive(false);
        }, this);
    }
}
