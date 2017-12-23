﻿using UnityEngine;

public class CircleController : MonoBehaviour
{
    public enum CircleState { Normal, DontPut, Happen }
    
    [SerializeField]
    SpriteRenderer spriteRenderer = null;

    [SerializeField]
    Sprite[] sprites = null;

    [SerializeField]
    float rotVal = 15.0f;
    
    [SerializeField]
    float interval = 0.25f;

    Transform m_transform;
    Vector3 rot;
    float t;

    BaseObjInputHandler.MakerType currentMakerType = BaseObjInputHandler.MakerType.None;

    BillboardSprite billboard;

    void Start()
    {
        m_transform = transform;
        rot = transform.eulerAngles;
        t = 0.0f;

        billboard = GetComponent<BillboardSprite>();
    }

    void Update()
    {
        if (currentMakerType != BaseObjInputHandler.MakerType.Normal) return;

        t += Time.deltaTime;

        if (t < interval) return;

        t = 0.0f;
        rot.y += rotVal;
        if (rot.y >= 360.0f)
        {
            rot.y = 0.0f;
        }
        m_transform.rotation = Quaternion.Euler(rot);
    }

    public void SetState(BaseObjInputHandler.MakerType makerType)
    {
        if (makerType == currentMakerType) return;

        currentMakerType = makerType;

        if (makerType == BaseObjInputHandler.MakerType.None) return;

        spriteRenderer.sprite = sprites[(int)makerType - 1];

        if (makerType == BaseObjInputHandler.MakerType.Normal)
        {
            transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
            billboard.enabled = false;
        }
        else
        {
            billboard.enabled = true;
            transform.eulerAngles = Vector3.zero;
        }
    }
}
