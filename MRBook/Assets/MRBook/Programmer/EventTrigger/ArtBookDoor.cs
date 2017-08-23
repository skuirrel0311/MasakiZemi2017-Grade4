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
        if (Input.GetKeyDown(KeyCode.D))
        {
            isHide = !isHide;
            m_renderer.enabled = !isHide;
            NotificationManager.I.ShowMessage("ドアが" + (isHide ? "開いた" : "閉じた"), 1.0f);
        }
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
