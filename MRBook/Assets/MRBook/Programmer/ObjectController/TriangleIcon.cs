using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleIcon : MonoBehaviour
{
    GameObject icon;

    public void Init(HoloObject owner)
    {
        //BoxCollider col = owner.m_collider;
        ////三角形の位置を決める
        //icon = Instantiate(ActorManager.I.trianglePrefab, transform);
        //icon.transform.localPosition = Vector3.up * col.size.y * 1.0f;
        //float scale = col.size.x * owner.transform.lossyScale.x * 0.5f;
        //scale = Mathf.Clamp(scale, 0.02f, 0.08f);
        //icon.transform.localScale = Vector3.one * scale * (1.0f / owner.transform.lossyScale.x);

        //MainSceneManager.I.OnPlayPage += (obj) => SetEnable(false);
        //MainSceneManager.I.OnReset += () => SetEnable(true);
    }

    void SetEnable(bool enabled)
    {
        icon.SetActive(enabled);
    }
}
