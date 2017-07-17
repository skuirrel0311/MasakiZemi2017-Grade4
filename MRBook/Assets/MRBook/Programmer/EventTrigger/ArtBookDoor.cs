using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtBookDoor : ArtBookGimmick
{
    [SerializeField]
    GameObject arrowSprite = null;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
#if UNITY_EDITOR

#else
        //初回だけ弾く
        if (!maker.IsVisuabled) return;

        if (arrowSprite.activeSelf != maker.IsVisuable)
        {
            arrowSprite.SetActive(maker.IsVisuable);
        }

        transform.position = maker.child.position;
#endif

    }


}
