using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtBookDoor : ArtBookGimmick
{
    Renderer m_renderer;

    protected override void Start()
    {
        base.Start();
        m_renderer = GetComponent<Renderer>();
    }

    protected override void Update()
    {
        base.Update();
        if (!maker.IsVisuabled) return;

        //隠れていたらenableを切る todo:ドアを閉めた時の場所に戻ると尚よい？
        m_renderer.enabled = !isHide;

        transform.position = maker.child.position;
    }
}
